using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLoop : MonoBehaviour
{
    public EventReference musicLoop;
    FMOD.Studio.EventInstance musicLoopinstance;

    public EventReference ambienceLoop;
    FMOD.Studio.EventInstance ambienceLoopinstance;

    public EventReference helmetLoop;
    FMOD.Studio.EventInstance helmetLoopinstance;

    public bool loopPlaying = false;

    public LevelTimer leveltimer;
    public PlayerConfig playerConfig;
    void Awake()
    {
        leveltimer = GetComponent<LevelTimer>();
        playerConfig = FindObjectOfType<PlayerConfig>();
    }
    public void Start()
    {
        if (!musicLoop.IsNull && !musicLoop.Equals("") && !musicLoopinstance.isValid()) {
            musicLoopinstance = FMODUnity.RuntimeManager.CreateInstance(musicLoop);
            loopPlaying = true;
            musicLoopinstance.start();
        } else {
            loopPlaying = true;
            musicLoopinstance.start();
        }
        if (!ambienceLoop.IsNull && !ambienceLoop.Equals("")) {
            ambienceLoopinstance = FMODUnity.RuntimeManager.CreateInstance(ambienceLoop);
            //ambienceLoopinstance.start();
        }
        if (!helmetLoop.IsNull && !helmetLoop.Equals("")) {
            helmetLoopinstance = FMODUnity.RuntimeManager.CreateInstance(helmetLoop);
            //helmetLoopinstance.start();
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
            if(playerConfig == null) {
                playerConfig = FindObjectOfType<PlayerConfig>();
            } else {
                //Global Vars
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Depth", playerConfig.depthAsPercent());
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("menuMusic", checkIfNotGameorTitle() ? 0 : 1);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("titleMusic", checkIfTitle() ? 1 : 0);
                //Debug.Log("titleMusic " + gameState.checkIfTitle());
                //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("remainingTime", leveltimer.getLevelTimeLeftPercent());
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isDead", playerConfig.playerState.isDead ? 1 : 0);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isRecalled", playerConfig.playerState.isRecalled ? 1 : 0);
            }
            //Debug.Log("isRecalled " + player.isRecalled);
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", playerController.getIntensity() / 100f);
        }
    }

    public bool checkIfTitle()
    {
        bool result = false;
        if (SceneManager.GetActiveScene().name == "TitleScene") {
            result =  true;
        }
        Debug.Log("checkIfTitle - " + result);
        return result;
    }

    public bool checkIfNotGameorTitle()
    {
        bool result = false;
        if (SceneManager.GetActiveScene().name != "TitleScene" || SceneManager.GetActiveScene().name != "LevelScene") {
            result = true;
        }
        Debug.Log("checkIfNotGameorTitle - " + result);
        return result;
    }

    public void startHelmet() {
        ambienceLoopinstance.setPaused(false);
        helmetLoopinstance.setPaused(false);
        ambienceLoopinstance.start();
        helmetLoopinstance.start();
    }

    public void stopHelmet() {
        ambienceLoopinstance.setPaused(true);
        helmetLoopinstance.setPaused(true);
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
        helmetLoopinstance.release();
    }
}
