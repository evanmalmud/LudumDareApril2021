using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckFMODLoaded : MonoBehaviour {
    public List<TMPro.TextMeshProUGUI> texts;

    public bool loadReady = false;

    public MusicLoopHandler musicLoopHandler;


    // Update is called once per frame
    void Update()
    {
        if (!loadReady && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            loadReady = true;
            updateTexts();
            //Debug.Log("Master Bank Loaded");
            //SceneManager.LoadScene("DemoScene", LoadSceneMode.Single);
        }
    }

    void updateTexts()
    {
        if (texts != null) {
            foreach (TMPro.TextMeshProUGUI text in texts) {
                if (text != null) {
                    text.text = "Click Here!";
                }
            }
        }
    }

    public void buttonClicked()
    {
        if (loadReady) {
            ResumeAudio();
            musicLoopHandler.startMusic();
            SceneManager.LoadScene("DemoScene", LoadSceneMode.Single);
        }
    }


    bool audioResumed = false;

    public void ResumeAudio()
    {
        if (!audioResumed) {
            var result = FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
            result = FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            audioResumed = true;
        }
    }
}
