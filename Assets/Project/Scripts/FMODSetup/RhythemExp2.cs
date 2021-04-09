using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RhythemExp2 : MonoBehaviour {

    class RythemTimeline {

        public float silentStartTime = 0f;
        public float normalStartTime = 0f;
        public float offsetTime = 0f;
        public List<MarkerEvent> markers = new List<MarkerEvent>();
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    class MarkerEvent {
        public string eventName = "";
        public float silentStartTime = 0f;
    }

    public List<PixelCircleCloser> pixelCircles;

    RythemTimeline timeline;
    GCHandle timelineHandle;
    FMOD.Studio.EVENT_CALLBACK beatCallback;

    private FMOD.Studio.EventInstance silentInstance;
    private FMOD.Studio.EventInstance normalInstance;

    [FMODUnity.EventRef]
    public string fmodSilentEvent;
    [FMODUnity.EventRef]
    public string fmodNormalEvent;

    public float offsetTimeSeconds = 2f;

    private bool loopPlaying = false;

    private const String LOOP_START = "LoopStart";
    private const String LOOP_END = "LoopEnd";
    private const String MARKER_PREFIX = "Marker";

    void Awake()
    {
        timeline = new RythemTimeline();
        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
    }
    public void startMusic()
    {
        pixelCircles = new List<PixelCircleCloser>(FindObjectsOfType<PixelCircleCloser>());
        loopPlaying = true;
        StartCoroutine("PlayFmodWithOffset");
    }

    // Update is called once per frame
    void Update()
    {
        //normalBus.setVolume(DecibelToLinear(normalBusVolume));
        if (!loopPlaying && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            startMusic();
        }
        //if(loopPlaying) {
        //    silentInstance.setVolume(0f);
        //}
        //LogTimelime();
    }

    void OnDestroy()
    {
        silentInstance.setUserData(IntPtr.Zero);
        silentInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        silentInstance.release();
        normalInstance.setUserData(IntPtr.Zero);
        normalInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        normalInstance.release();
        timelineHandle.Free();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
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
            RythemTimeline timelineInfo = (RythemTimeline)timelineHandle.Target;

            switch (type) {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER: {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        if(((String)parameter.name).Contains(MARKER_PREFIX)) {
                            StartCoroutine(ListenForInput(parameter.name, parameter.position));
                        }
                        //timelineInfo.lastMarker = parameter.name;
                        //timelineInfo.currentPosition = parameter.position;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }


    IEnumerator PlayFmodWithOffset()
    {
        silentInstance = FMODUnity.RuntimeManager.CreateInstance(fmodSilentEvent);
        normalInstance = FMODUnity.RuntimeManager.CreateInstance(fmodNormalEvent);
        // Pin the class that will store the data modified during the callback
        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timeline, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        silentInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        // Pass the object through the userdata of the instance
        //instance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        silentInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.ALL);
        //silentInstance.setVolume(0);
        silentInstance.setPaused(true);
        silentInstance.start();
        normalInstance.setPaused(true);
        normalInstance.start();
        silentInstance.setPaused(false);
        yield return new WaitForSeconds(offsetTimeSeconds);
        normalInstance.setPaused(false);
    }

    IEnumerator ListenForInput(String markerName, int pos)
    {
        Debug.Log("Listen For input - " + markerName + " Pos: " + pos + " At Time : " + Time.time);
        yield return new WaitForSeconds(offsetTimeSeconds);
        Debug.Log("Expected input - " + markerName + " At Time : " + Time.time);
    }


    private float DecibelToLinear(float dB) {
        float linear = Mathf.Pow(10.0f, dB / 20f);
        return linear;
    }
}
