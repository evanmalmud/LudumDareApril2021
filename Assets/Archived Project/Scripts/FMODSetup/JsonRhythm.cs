using Sirenix.OdinInspector;
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

        public string markerName;
        public double startPosition;
        public double endPosition;

        public string getKeyName() {
            return markerName.Replace(MARKER_PREFIX, "").Replace(END_PREFIX, "").Replace(START_PREFIX, "").Trim();
        }

        public static int SortByStartPosition(Marker p1, Marker p2)
        {
            return p1.startPosition.CompareTo(p2.startPosition);
        }
    }

    [FilePath]
    public string callbackJson;

    public CallbackMarkers callbackMarkers;

    [SerializeField]
    public Queue<Marker> queueMarkers = new Queue<Marker>();
    [SerializeField]
    List<Marker> toBeQueued = new List<Marker>();

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

    //Used for instant button presses. One shots
    private const String MARKER_PREFIX = "Marker";

    // Used for Starts and ends of held beats.
    private const String START_PREFIX = "Start";
    private const String END_PREFIX = "End";

    [System.Serializable]
    public class RhythmSettings {
        [SerializeField]
        public double animationCountdownTime = .5d;
        [SerializeField]
        public double preInputTime = .1d;
        [SerializeField]
        public double postInputTime = .1d;
        [SerializeField]
        public double videoaudiobuffer = .175d;
    }

    [SerializeField]
    public RhythmSettings rhythmSettings = new RhythmSettings();

        // Graphics 

    public Dictionary<String, RhythmObjectManager> pixelCircles = new Dictionary<string, RhythmObjectManager>();

    public bool logTime = false;

    private RhythmInput rhythmInput;
    private void Awake()
    {
        rhythmInput = GetComponent<RhythmInput>();
        coreSystem = FMODUnity.RuntimeManager.CoreSystem;
        coreSystem.getMasterChannelGroup(out channelGroup);
        coreSystem.getSoftwareFormat(out sampleRate, out speakerMode, out numRawSpeakers);
        sampleRateDouble = Convert.ToDouble(sampleRate);

        foreach(RhythmObjectManager pcc in FindObjectsOfType<RhythmObjectManager>()) {
            pixelCircles.Add(pcc.keyName, pcc);
        }

        using (StreamReader stream = new StreamReader(callbackJson)) {
            string json = stream.ReadToEnd();
            callbackMarkers = JsonUtility.FromJson<CallbackMarkers>(json);
            Dictionary<String, int> currentStartMarkersInQueue = new Dictionary<string, int>();
            List<Marker> sortedListOfOrigionalMarkers = new List<Marker>();
            sortedListOfOrigionalMarkers.AddRange(callbackMarkers.markers);
            sortedListOfOrigionalMarkers.Sort(Marker.SortByStartPosition);
            foreach (Marker marker in sortedListOfOrigionalMarkers) {
                if(marker.markerName.StartsWith(MARKER_PREFIX)) {
                    toBeQueued.Add(marker);
                } else if (marker.markerName.StartsWith(START_PREFIX)) {
                    //Add to queue and currentStartMarkersInQueue
                    toBeQueued.Add(marker);
                    if(currentStartMarkersInQueue.ContainsKey(marker.getKeyName())) {
                        //If it already contains a startKey. Treat the first as a one shot marker
                        currentStartMarkersInQueue.Remove(marker.getKeyName());
                    }
                    currentStartMarkersInQueue.Add(marker.getKeyName(), toBeQueued.IndexOf(marker));
                } else if (marker.markerName.StartsWith(END_PREFIX)) {
                    //Find in currentStartMarkersInQueue and update queue
                    int index = -1;
                    currentStartMarkersInQueue.TryGetValue(marker.getKeyName(), out index);
                    if(index >= 0) {
                        toBeQueued[index].endPosition = marker.startPosition;
                        currentStartMarkersInQueue.Remove(marker.getKeyName());
                    }
                }
            }
            toBeQueued.Sort(Marker.SortByStartPosition);
            foreach(Marker marker in toBeQueued) {
                queueMarkers.Enqueue(marker);
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
                if((currentUnityTime - dspaudioOffset) > (queueMarkers.Peek().startPosition - rhythmSettings.animationCountdownTime) ||
                    IsApproximatelyEqualTo(currentUnityTime - dspaudioOffset, queueMarkers.Peek().startPosition - rhythmSettings.animationCountdownTime)) {
                    {
                        RhythmObjectManager pcc;
                        if(pixelCircles.TryGetValue(queueMarkers.Peek().getKeyName(), out pcc)) {
                            pcc.markerRhythm(rhythmInput);
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
