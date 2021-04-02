using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoopHandler : MonoBehaviour
{

    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    public LevelTimer levelTimer;

    public PlayerController playerController;

    private bool loopPlaying = false;

    // Start is called before the first frame update
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
        if (loopPlaying) {
            instance.setParameterByName("Vertigo", playerController.getVertigo() ? 1 : 0);
            instance.setParameterByName("Health", playerController.getHealthPercentage());
            instance.setParameterByName("Time", levelTimer.getLevelTimeLeft());
            //Global Vars
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("HasLost", playerController.getDead() ? 1 : 0);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", playerController.getIntensity() / 100f);
        }
    }

    public void restartMusic() {
        instance.start();
    }
}
