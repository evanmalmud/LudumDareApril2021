using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MusicLoopHandler : MonoBehaviour {

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

    void Awake()
    {

    }
    public void startMusic()
    {

        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
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

        //LogTimelime();
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
    }
}
