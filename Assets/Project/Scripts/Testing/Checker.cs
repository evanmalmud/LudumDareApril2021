using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{

    public GameObject gameManagerPrefab;

    private GameObject gm;

    CheckFMODLoaded fmodLoad;
    MusicLoopHandler loopHandler;
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<MusicLoopHandler>() == null) {
            gm = Instantiate(gameManagerPrefab);
            fmodLoad = gm.GetComponent<CheckFMODLoaded>();
            loopHandler = gm.GetComponent<MusicLoopHandler>();
        } else {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if(fmodLoad.loadReady) {
            fmodLoad.ResumeAudio();
            loopHandler.startMusic();
            Destroy(this.gameObject);
        }
    }
}
