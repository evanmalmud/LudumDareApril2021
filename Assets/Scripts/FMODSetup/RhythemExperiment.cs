using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RhythemExperiment : MonoBehaviour {

    // Variables that are modified in the callback need to be part of a seperate class.
    // This class needs to be 'blittable' otherwise it can't be pinned in memory.
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo {

        public int currentMusicBar = 0;
        public int currentMusicBeat = 0;
        public int currentPosition = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    class LoopInfo {
        public int LoopStartMusicBar = 0;
        public int LoopStartMusicBeat = 0;
        public int LoopEndMusicBar = 0;
        public int LoopEndMusicBeat = 0;
        public List<Marker> markers = new List<Marker>();
        public String lastMarker;
        public Marker currentMarker;
    }

    class Marker {
        public int position = 0;
        public String markerName;
    }

    public List<PixelCircleCloser> pixelCircles;

    public int positionBuffer = 50;

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    LoopInfo currentLoop;

    FMOD.Studio.EVENT_CALLBACK beatCallback;

   
    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    public LevelTimer levelTimer;

    private bool loopPlaying = false;

    private const String LOOP_START = "LoopStart";
    private const String LOOP_END = "LoopEnd";
    public bool firstStartHit = false;
    public bool firstEndHit = false;
    public bool SecondStartHit = false;
    public bool SecondEndHit = false;

    void Awake()
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

        instance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.ALL);
        instance.start();

        loopPlaying = true;

        pixelCircles = new List<PixelCircleCloser>(FindObjectsOfType<PixelCircleCloser>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!loopPlaying && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            startMusic();
        }
        LogTimelime();
    }

    public void restartMusic()
    {
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

        int position = 0;
        instance.getTimelinePosition(out position);
        if (currentLoop == null || currentLoop.currentMarker == null) {
            //Waiting for Next Loop
        } else if (SecondStartHit && position >= (currentLoop.currentMarker.position - positionBuffer)) {
            Debug.Log("Current Position - " + position);
            Debug.Log("StartPixelCircle for " + currentLoop.currentMarker.markerName + "  position " + position);

            foreach (PixelCircleCloser pixelCircle in pixelCircles) {
                if (pixelCircle.name.Equals(currentLoop.currentMarker.markerName)) {
                    //pixelCircle.startTween();
                }
            }

            //Advance counter
            int currentIndex = currentLoop.markers.IndexOf(currentLoop.currentMarker);
            if (currentIndex + 1 >= currentLoop.markers.Count) {
                currentLoop.currentMarker = null;
            } else {
                currentLoop.currentMarker = currentLoop.markers[currentIndex + 1];
            }
        } else if (SecondStartHit && position >= (currentLoop.currentMarker.position - (positionBuffer * 1.5))) {
            Debug.Log("Current Position - " + position);
        } else if (SecondStartHit) {
            foreach (Marker marker in currentLoop.markers) {
                if (timelineInfo.currentPosition == marker.position &&
                    !((string)timelineInfo.lastMarker).Equals(currentLoop.lastMarker)) {
                    Debug.Log("Expecting Player input for - " + marker.markerName + " Position " + position);
                    currentLoop.lastMarker = marker.markerName;
                }
            }
        }
        //Debug.Log("position " + position);
        if (((string)timelineInfo.lastMarker).Equals(LOOP_START)) {
            if (SecondStartHit) {
                instance.setParameterByName("PlayerInput", SecondStartHit ? 1 : 0);
            } else if (firstStartHit && firstEndHit) {
                SecondStartHit = true;
            } else if (firstStartHit) {
                //TODO: Reset currentLoop if loop Listening true while seeing new loop
                //Check for different LOOP NAME
            } else {
                Debug.Log("firstStartHit Loop");
                currentLoop.LoopStartMusicBar = timelineInfo.currentMusicBar;
                currentLoop.LoopStartMusicBeat = timelineInfo.currentMusicBeat;
                firstStartHit = true;
            }
        } else if (((string)timelineInfo.lastMarker).Equals(LOOP_END)) {
            if (SecondStartHit) {
                SecondEndHit = true;
                //TODO: ResetLoop?
            } else if (firstStartHit) {
                Debug.Log("End Loop");
                currentLoop.LoopEndMusicBar = timelineInfo.currentMusicBar;
                currentLoop.LoopEndMusicBeat = timelineInfo.currentMusicBeat;
                firstEndHit = true;
                currentLoop.currentMarker = currentLoop.markers[0];
            }
        } else if (firstStartHit && ((string)timelineInfo.lastMarker).Contains("Marker") &&
            !((string)timelineInfo.lastMarker).Equals(currentLoop.lastMarker)) {
            //Add Marker to List
            Marker marker = new Marker();
            marker.markerName = (string)timelineInfo.lastMarker;
            marker.position = timelineInfo.currentPosition;
            currentLoop.markers.Add(marker);
            currentLoop.lastMarker = (string)timelineInfo.lastMarker;
            Debug.Log((string)timelineInfo.lastMarker + "  position " + position);
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
                        timelineInfo.currentPosition = parameter.position;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }
}
