using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public bool intro = true;
    public DialogueScriptableObject initialDialogue;

    public TMPro.TextMeshProUGUI textBox;

    public GameObject skipButton;

    [FMODUnity.EventRef]
    public string rocketAmbience;

    public GameObject ship;

    FMOD.Studio.EventInstance initialDialogueInstance;
    FMOD.Studio.EventInstance rocketAmbienceInstance;

    Transform startingtransform;
    public void Start()
    {
        startingtransform = ship.transform;
        ship.SetActive(false);
        skipButton.SetActive(false);
        ship.transform.localScale = new Vector3(.5f, .5f, 1);
        textBox.text = "";
        if (!initialDialogue.sfxName.Equals(null) && !initialDialogue.sfxName.Equals("")) {
            initialDialogueInstance = FMODUnity.RuntimeManager.CreateInstance(initialDialogue.sfxName);
        }

        if (!rocketAmbience.Equals(null) && !rocketAmbience.Equals("")) {
            rocketAmbienceInstance = FMODUnity.RuntimeManager.CreateInstance(rocketAmbience);
        }
    }

    public void cancelDialogue() {
        StopCoroutine("playDialogue");
        playShipOffScreen();
        rocketAmbienceInstance.setPaused(true);
        initialDialogueInstance.setPaused(true);
        rocketAmbienceInstance.release();
        initialDialogueInstance.release();
    }

    public void playIntroDialogue() {
        ship.SetActive(true);
        ship.transform.position = startingtransform.position;
        skipButton.SetActive(true);
        rocketAmbienceInstance.start();
        StartCoroutine("playDialogue");
    }

    public void playShipZoom(){
        ship.transform.DOScale(new Vector3(1f, 1f, 1f), 6f);
    }

    public void playShipOffScreen() {
        ship.transform.DOMoveX(ship.transform.position.x - 2, 0.6f).OnComplete(playShipOffScreen2);
    }
    public void playShipOffScreen2() {
        ship.transform.DOMoveX(ship.transform.position.x + 25, 2f);
    }

    IEnumerator playDialogue() {
        yield return new WaitForSeconds(.5f);
        playShipZoom();
        initialDialogueInstance.start();
        for(int i = 0; i < initialDialogue.dialogueSentences.Count; i++) {
            textBox.text = initialDialogue.dialogueSentences[i];
            yield return new WaitForSeconds(initialDialogue.sentenceWaitTimes[i]);
            if(i == initialDialogue.dialogueSentences.Count - 2) {
                //Play ship anims
                playShipOffScreen();
            }
        }
        if(intro) {
            FindObjectOfType<GameState>().dialogueComplete();
        } else {
            FindObjectOfType<GameState>().midGameDialogueSkipp();
        }
        rocketAmbienceInstance.setPaused(true);
        rocketAmbienceInstance.release();
        initialDialogueInstance.release();
        yield return null;
    }
}
