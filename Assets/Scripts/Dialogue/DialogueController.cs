using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{

    public DialogueScriptableObject initialDialogue;

    public TMPro.TextMeshProUGUI textBox;

    [FMODUnity.EventRef]
    public string rocketAmbience;

    FMOD.Studio.EventInstance initialDialogueInstance;
    FMOD.Studio.EventInstance rocketAmbienceInstance;
    public void Start()
    {
        textBox.text = "";
        if (!initialDialogue.sfxName.Equals(null) && !initialDialogue.sfxName.Equals("")) {
            initialDialogueInstance = FMODUnity.RuntimeManager.CreateInstance(initialDialogue.sfxName);
        }

        if (!rocketAmbience.Equals(null) && !rocketAmbience.Equals("")) {
            rocketAmbienceInstance = FMODUnity.RuntimeManager.CreateInstance(rocketAmbience);
        }
    }

    public void playIntroDialogue() {
        rocketAmbienceInstance.start();
        StartCoroutine("playDialogue");
    }

    IEnumerator playDialogue() {
        yield return new WaitForSeconds(.5f);
        initialDialogueInstance.start();
        for(int i = 0; i < initialDialogue.dialogueSentences.Count; i++) {
            textBox.text = initialDialogue.dialogueSentences[i];
            yield return new WaitForSeconds(initialDialogue.sentenceWaitTimes[i]);
        }
        FindObjectOfType<GameState>().dialogueComplete();
        rocketAmbienceInstance.setPaused(true);
        rocketAmbienceInstance.release();
        initialDialogueInstance.release();
        yield return null;
    }
}
