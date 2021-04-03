using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MusicLoopHandler : MonoBehaviour
{

    // Variables that are modified in the callback need to be part of a seperate class.
    // This class needs to be 'blittable' otherwise it can't be pinned in memory.
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo {

        public int currentMusicBar = 0;
        public int currentMusicBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    class LoopInfo {
        public int LoopStartMusicBar = 0;
        public int LoopStartMusicBeat = 0;
        public int LoopEndMusicBar = 0;
        public int LoopEndMusicBeat = 0;
        public List<Marker> markers = new List<Marker>();
        public String lastMarker;
    }

    class Marker {
        public int musicBar = 0;
        public int musicBeat = 0;
        public String markerName;
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    LoopInfo currentLoop;

    FMOD.Studio.EVENT_CALLBACK beatCallback;

    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    public LevelTimer levelTimer;

    public PlayerController playerController;

    private bool loopPlaying = false;

    private const String LOOP_START = "LoopStart";
    private const String LOOP_END = "LoopEnd";
    public bool loopListening = false;
    public bool playerListening = false;

    void Start()
    {
        timelineInfo = new TimelineInfo();
        currentLoop = new LoopInfo();
        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
    }
    public void startMusic()
    {

        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        instance.setUserData(GCHandle.ToIntPtr(timelineHandle));

        instance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        instance.start();
        loopPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController == null) {
            if (FindObjectOfType<PlayerController>() != null) {
                playerController = FindObjectOfType<PlayerController>();
                playerController.musicLoopHandler = this;
            }
        }
        //Local Vars
        if (loopPlaying && playerController) {
            instance.setParameterByName("Vertigo", playerController.getVertigo() ? 1 : 0);
            instance.setParameterByName("Health", playerController.getHealthPercentage());
            instance.setParameterByName("Time", levelTimer.getLevelTimeLeft());
            //Global Vars
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("HasLost", playerController.getDead() ? 1 : 0);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", playerController.getIntensity() / 100f);
        }

        LogTimelime();
    }

    public void restartMusic() {
        instance.start();
    }

    void OnDestroy()
    {
        instance.setUserData(IntPtr.Zero);
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
        timelineHandle.Free();
    }

    void OnGUI()
    {
        GUILayout.Box(String.Format("Current Beat = {0}, Current Bar = {1}, Last Marker = {2}",
        timelineInfo.currentMusicBeat,
        timelineInfo.currentMusicBar,
        (string)timelineInfo.lastMarker));
    }


    void LogTimelime()
    {

        if (((string)timelineInfo.lastMarker).Equals(LOOP_START)) {
            if (playerListening) {
                instance.setParameterByName("PlayerInput", playerListening ? 1 : 0);
            } else if (loopListening) {
                //TODO: Reset currentLoop if loop Listening true while seeing new loop
            } else {
                Debug.Log("Start Loop");
                currentLoop.LoopStartMusicBar = timelineInfo.currentMusicBar;
                currentLoop.LoopStartMusicBeat = timelineInfo.currentMusicBeat;
                loopListening = true;
            }
        } else if (((string)timelineInfo.lastMarker).Equals(LOOP_END)) {
            if(playerListening) {
                //TODO: ResetLoop?
            } else if (loopListening) {
                Debug.Log("End Loop");
                currentLoop.LoopEndMusicBar = timelineInfo.currentMusicBar;
                currentLoop.LoopEndMusicBeat = timelineInfo.currentMusicBeat;
                loopListening = false;
                playerListening = true;
            }
        } else if (loopListening && ((string)timelineInfo.lastMarker).Contains("Marker") &&
            !((string)timelineInfo.lastMarker).Equals(currentLoop.lastMarker)) {
            //Add Marker to List
            Marker marker = new Marker();
            marker.markerName = (string)timelineInfo.lastMarker;
            marker.musicBar = timelineInfo.currentMusicBar;
            marker.musicBeat = timelineInfo.currentMusicBeat;
            currentLoop.markers.Add(marker);
            currentLoop.lastMarker = (string)timelineInfo.lastMarker;
            Debug.Log((string)timelineInfo.lastMarker);
        } else if(playerListening) {
            foreach(Marker marker in currentLoop.markers) {
                if (timelineInfo.currentMusicBeat == marker.musicBeat &&
                    timelineInfo.currentMusicBar == marker.musicBar &&
                    !((string)timelineInfo.lastMarker).Equals(currentLoop.lastMarker)) {
                    Debug.Log("Expecting Player input for - " + marker.markerName);
                    currentLoop.lastMarker = marker.markerName;
                    }
            }
        }
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK) {
            Debug.LogError("Timeline Callback error: " + result);
        } else if (timelineInfoPtr != IntPtr.Zero) {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type) {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT: {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBeat = parameter.beat;
                        timelineInfo.currentMusicBar = parameter.bar;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER: {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }
}
