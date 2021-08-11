using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScreenInteract : PlayerInteractable {

    public ShipInteriorManager shipInteriorManager;

    override public void Interact()
    {
        //Zoom in camera
        shipInteriorManager.ZoomCameraToLevelSelect();
    }
}
