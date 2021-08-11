using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : Box2DToggle {

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (layermask == (layermask | (1 << collision.gameObject.layer))) {
            ShipInteriorConfig shipInteriorConfig = collision.gameObject.GetComponent<ShipInteriorConfig>();
            if(shipInteriorConfig != null) {
                shipInteriorConfig.setCurrentInteractable(this);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (layermask == (layermask | (1 << collision.gameObject.layer))) {
            //disable light
            ShipInteriorConfig shipInteriorConfig = collision.gameObject.GetComponent<ShipInteriorConfig>();
            if (shipInteriorConfig != null) {
                shipInteriorConfig.removeCurrentInteractable(this);
            }
        }
    }

    virtual public void Interact()
    {
        
    }
}
