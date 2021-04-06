using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckFMODLoaded : MonoBehaviour {
    public List<TMPro.TextMeshProUGUI> texts;

    public bool loadReady = false;

    public MusicLoopHandler musicLoopHandler;

    [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
    public string SceneName;

    // Update is called once per frame
    void Update()
    {
        if (!loadReady && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            loadReady = true;
            updateTexts();
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
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
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
