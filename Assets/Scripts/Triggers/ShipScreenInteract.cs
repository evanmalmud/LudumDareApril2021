using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScreenInteract : PlayerInteractable {

    public ShipInteriorManager shipInteriorManager;

    public bool interactActive = false;

    override public void Interact()
    {
        if (!interactActive) {
            //Zoom in camera
            shipInteriorManager.ZoomCameraToLevelSelect();
            interactActive = true;
        } else {
            //Load level
            FindObjectOfType<SceneLoader>().LoadScene();
        }
    }

    override public void UnInteract()
    {
        interactActive = false;
    }
}
