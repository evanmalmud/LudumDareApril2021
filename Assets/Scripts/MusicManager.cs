using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    public LevelTimer levelTimer;

    public PlayerController playerController;

    private bool loopPlaying = false;

    public void Awake()
    {
        startMusic();
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
                playerController.musicManager = this;
            }
        }
        //Local Vars
        if (loopPlaying && playerController) {
            instance.setParameterByName("Vertigo", playerController.getVertigo() ? 1 : 0);
            instance.setParameterByName("Health", playerController.getHealthPercentage());
            if (levelTimer != null) {
                instance.setParameterByName("Time", levelTimer.getLevelTimeLeft());
            }
            //Global Vars
            if (playerController != null) {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("HasLost", playerController.getDead() ? 1 : 0);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", playerController.getIntensity() / 100f);
            }
        }
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
