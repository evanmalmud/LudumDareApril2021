using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject titleCanvas;

    public GameObject dialogueIntroCanvas;

    public GameObject mainCamera;
    public CameraFollow mainCameraFollow;

    public GameObject currentObject;
    public CanvasHelper currentCanvasHelper;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCameraFollow = mainCamera.GetComponent<CameraFollow>();
    }
    public void transitionToDialogue()
    {
        transition(dialogueIntroCanvas);
        DialogueController dialogueController = dialogueIntroCanvas.GetComponent<DialogueController>();
        dialogueController.playIntroDialogue();
    }

    public void transition(GameObject go) {
        if(currentObject == go) {
            //We are already here
            return;
        }
      
        if (currentCanvasHelper != null) {
            //TODO: Play out sfx
        }
        mainCameraFollow.target = go.transform;
        CanvasHelper canvasHelper = go.GetComponent<CanvasHelper>();

        if(canvasHelper != null) {
            //TODO: Play in sfx
        }


        currentObject = go;
        currentCanvasHelper = canvasHelper;
    }
}
