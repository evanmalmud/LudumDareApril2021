using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string musicLoop;
    private FMOD.Studio.EventInstance musicLoopinstance;
    [FMODUnity.EventRef]
    public string ambienceLoop;
    private FMOD.Studio.EventInstance ambienceLoopinstance;
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
        if (musicLoop != null && !musicLoop.Equals("")) {
            musicLoopinstance = FMODUnity.RuntimeManager.CreateInstance(musicLoop);
            musicLoopinstance.start();
            loopPlaying = true;
        }
        if (ambienceLoop != null && !ambienceLoop.Equals("")) {
            ambienceLoopinstance = FMODUnity.RuntimeManager.CreateInstance(ambienceLoop);
            ambienceLoopinstance.start();
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
        musicLoopinstance.start();
        ambienceLoopinstance.start();
    }

    void OnDestroy()
    {
        musicLoopinstance.setUserData(IntPtr.Zero);
        musicLoopinstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicLoopinstance.release();
        ambienceLoopinstance.release();
    }
}
