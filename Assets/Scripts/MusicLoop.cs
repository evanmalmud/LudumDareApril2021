using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    //public LevelTimer levelTimer;

    bool loopPlaying = false;


    LevelTimer leveltimer;
    GameState gameState;
    Player player;
    void Awake()
    {
        leveltimer = GetComponent<LevelTimer>();
        gameState = GetComponent<GameState>();
        player = FindObjectOfType<Player>();
    }
    public void startMusic()
    {
        if (fmodEvent != null && !fmodEvent.Equals("")) {
            instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
            instance.start();
            loopPlaying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Local Vars
        if (loopPlaying) {
            //instance.setParameterByName("Vertigo", playerController.getVertigo() ? 1 : 0);
            //instance.setParameterByName("Health", playerController.getHealthPercentage());
            //instance.setParameterByName("Time", levelTimer.getLevelTimeLeft());
            //Global Vars
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Depth", player.depthAsPercent());
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MenuMusic", gameState.checkIfNotGame() ? 0 : 1);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("remainingTime", leveltimer.getLevelTimeLeftPercent());
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isDead", player.isDead ? 1 : 0);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isRecalled", player.isRecalled ? 1 : 0);
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", playerController.getIntensity() / 100f);
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
