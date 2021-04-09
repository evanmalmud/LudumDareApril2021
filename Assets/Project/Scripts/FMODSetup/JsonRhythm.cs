using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonRhythm : MonoBehaviour
{

    // JSON VARS
    
    [System.Serializable]
    public class CallbackMarkers {
        public Marker[] markers;
    }
    [System.Serializable]
    public class Marker {

        public string name;
        public double position;
    }

    [SerializeField]
    public string callbackJson;

    public CallbackMarkers callbackMarkers;

    public Queue<Marker> queueMarkers = new Queue<Marker>();

    // AUDIO VARS
    static FMOD.System coreSystem;
    FMOD.ChannelGroup channelGroup;
    int sampleRate, numRawSpeakers;
    FMOD.SPEAKERMODE speakerMode;
    double sampleRateDouble;
    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    public double currentUnityTime;

    public double dspaudioOffset = 0d;

    public bool loopPlaying = false;
    public bool startMusicbool = false;

    private const String MARKER_PREFIX = "Mark";

    public double pixelcirlceOffsetTime = 0.5d;

    // Graphics 

    public Dictionary<String, PixelCircleCloser> pixelCircles = new Dictionary<string, PixelCircleCloser>();

    public bool logTime = false;

    private RhythmInput rhythmInput;
    private void Awake()
    {
        rhythmInput = GetComponent<RhythmInput>();
        coreSystem = FMODUnity.RuntimeManager.CoreSystem;
        coreSystem.getMasterChannelGroup(out channelGroup);
        coreSystem.getSoftwareFormat(out sampleRate, out speakerMode, out numRawSpeakers);
        sampleRateDouble = Convert.ToDouble(sampleRate);

        foreach(PixelCircleCloser pcc in FindObjectsOfType<PixelCircleCloser>()) {
            pixelCircles.Add(pcc.markerName, pcc);
        }

        using (StreamReader stream = new StreamReader(callbackJson)) {
            string json = stream.ReadToEnd();
            callbackMarkers = JsonUtility.FromJson<CallbackMarkers>(json);
            foreach(Marker marker in callbackMarkers.markers) {
               // Debug.Log(marker.name + "  " + marker.position.ToString());
                if(marker.name.StartsWith(MARKER_PREFIX)) {
                    //Debug.Log("Before" + queueMarkers.Count);
                    queueMarkers.Enqueue(marker);
                    //Debug.Log("After" + queueMarkers.Count);
                }
            }
            //Debug.Log(queueMarkers.Count);
        }
    }

    public void startMusic()
    {
        loopPlaying = true;
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (logTime) {
            Debug.Log("LogTime " + currentUnityTime);
            logTime = false;
        }

        if (startMusicbool && !loopPlaying && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            startMusic();
        } else if (loopPlaying) {
            double dspClockTime = getDSPClockTime();
            //FIRST time
            if (startMusicbool) {
                int position;
                instance.getTimelinePosition(out position);
                dspaudioOffset = dspClockTime - (position / 1000);
                startMusicbool = false;
            }
            currentUnityTime += Time.deltaTime;
            if (IsApproximatelyEqualTo(dspClockTime,currentUnityTime)) {
               //Debug.Log("Clocks are approximately the same");
            } else {
                //Debug.Log("Clocks are offset");
                //Debug.Log("Clocks are offset DSP: " + dspClockTime + " Unity: " + currentUnityTime);
                currentUnityTime = dspClockTime;
            }
            //Debug.Log(currentUnityTime);
            if (queueMarkers.Count > 0) {
                //Debug.Log("Peek: " + queueMarkers.Peek().name + " " + queueMarkers.Peek().position);
                if((currentUnityTime - dspaudioOffset) > (queueMarkers.Peek().position - pixelcirlceOffsetTime) ||
                    IsApproximatelyEqualTo(currentUnityTime - dspaudioOffset, queueMarkers.Peek().position - pixelcirlceOffsetTime)) {
                    {
                        PixelCircleCloser pcc;
                        if(pixelCircles.TryGetValue(queueMarkers.Peek().name, out pcc)) {
                            pcc.startTween(rhythmInput);
                        }
                        queueMarkers.Dequeue();
                    }
                    /*
                    if (currentUnityTime - dspaudioOffset > queueMarkers.Peek().position ||
                    IsApproximatelyEqualTo(currentUnityTime - dspaudioOffset, queueMarkers.Peek().position)) {
                    Debug.Log("Event Time - Name: " + queueMarkers.Peek().name +
                        " Pos: " + queueMarkers.Peek().position);
                    // TRIGGER EVENT
                    queueMarkers.Dequeue();*/
                }
            }
        }
    }



    public double getDSPClockTime() {
        ulong dspClock, parentclock;
        channelGroup.getDSPClock(out dspClock, out parentclock);
        double dspDivSamples = Convert.ToDouble(dspClock) / sampleRateDouble;
        double parentDivSamples = Convert.ToDouble(parentclock) / sampleRateDouble;
        return dspDivSamples;
    }


    public static bool IsApproximatelyEqualTo(double initialValue, double value)
    {
        return IsApproximatelyEqualTo(initialValue, value, 0.0019);
    }

    public static bool IsApproximatelyEqualTo(double initialValue, double value, double maximumDifferenceAllowed)
    {
        // Handle comparisons of floating point values that may not be exactly the same
        return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
    }
}
