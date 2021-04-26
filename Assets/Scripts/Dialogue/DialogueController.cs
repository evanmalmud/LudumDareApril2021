using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{

    public DialogueScriptableObject initialDialogue;

    public TMPro.TextMeshProUGUI textBox;

    FMOD.Studio.EventInstance initialDialogueInstance;
    public void Start()
    {
        textBox.text = "";
        if (!initialDialogue.sfxName.Equals(null) && !initialDialogue.sfxName.Equals("")) {
            initialDialogueInstance = FMODUnity.RuntimeManager.CreateInstance(initialDialogue.sfxName);
        }
    }

    public void playIntroDialogue() {
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
        yield return null;
    }
}
