using DG.Tweening;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public bool intro = true;
    public DialogueScriptableObject initialDialogue;

    public TMPro.TextMeshProUGUI textBox;

    public GameObject skipButton;

    public EventReference rocketAmbience;
    FMOD.Studio.EventInstance rocketAmbienceInstance;

    public GameObject ship;

    FMOD.Studio.EventInstance initialDialogueInstance;

    public Vector3 startingtransform;
    public void Start()
    {
        startingtransform = ship.transform.position;
        skipButton.SetActive(false);
        ship.transform.localScale = new Vector3(.5f, .5f, 1);
        textBox.text = "";

    }

    public void cancelDialogue() {
        StopCoroutine("playDialogue");
        playShipOffScreen();
        skipButton.SetActive(false);
        textBox.enabled = false;
        ship.transform.position = startingtransform;
        rocketAmbienceInstance.setPaused(true);
        initialDialogueInstance.setPaused(true);
        rocketAmbienceInstance.release();
        initialDialogueInstance.release();
        if (intro) {
            FindObjectOfType<SceneLoader>().LoadScene();
        }
    }

    public void playIntroDialogue() {
        if (!initialDialogue.sfxName.Equals(null) && !initialDialogue.sfxName.Equals("")) {
            initialDialogueInstance = FMODUnity.RuntimeManager.CreateInstance(initialDialogue.sfxName);
        }

        if (!rocketAmbience.Equals(null) && !rocketAmbience.Equals("")) {
            rocketAmbienceInstance = FMODUnity.RuntimeManager.CreateInstance(rocketAmbience);
        }
        ship.transform.position = startingtransform;
        skipButton.SetActive(true);
        textBox.enabled = true;
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
        ship.transform.DOMoveX(ship.transform.position.x + 25, 2f).OnComplete(moveShip);
    }

    public void moveShip() {
        StartCoroutine("moveShipCour");
    }

    IEnumerator moveShipCour()
    {
        yield return new WaitForSeconds(2f);
        ship.transform.position = startingtransform;
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
            FindObjectOfType<SceneLoader>().LoadScene();
        }/* else {
            FindObjectOfType<GameState>().midGameDialogueSkipp();
            skipButton.SetActive(false);
            textBox.enabled = false;
            ship.transform.position = startingtransform;
        }*/
        rocketAmbienceInstance.setPaused(true);
        rocketAmbienceInstance.release();
        initialDialogueInstance.setPaused(true);
        initialDialogueInstance.release();

        yield return null;
    }
}
