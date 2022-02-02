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

    public PlayerConfig playerConfig;

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
        if (playerConfig == null) {
            if (FindObjectOfType<PlayerController>() != null) {
                playerConfig = FindObjectOfType<PlayerConfig>();
            }
        }
        //Local Vars
        if (loopPlaying && playerConfig) {
            //Global Vars
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Depth", playerConfig.depthAsPercent());
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("menuMusic", gameState.checkIfNotGameorTitle() ? 0 : 1);
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("titleMusic", gameState.checkIfTitle() ? 1 : 0);
            //Debug.Log("titleMusic " + gameState.checkIfTitle());
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("remainingTime", leveltimer.getLevelTimeLeftPercent());
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isDead", playerConfig.playerState.isDead ? 1 : 0);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isRecalled", playerConfig.playerState.isRecalled ? 1 : 0);

            //instance.setParameterByName("Vertigo", playerConfig.getVertigo() ? 1 : 0);
            //instance.setParameterByName("Health", playerConfig.getHealthPercentage());
            if (levelTimer != null) {
                instance.setParameterByName("Time", levelTimer.getLevelTimeLeft());
            }
            //Global Vars
            if (playerConfig != null) {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("HasLost", playerConfig.playerState.isDead ? 1 : 0);
                //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", playerConfig.getIntensity() / 100f);
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
