using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : Box2DToggle {

    // Update is called once per frame
    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (layermask == (layermask | (1 << collision.gameObject.layer))) {
            PlayerConfig playerConfig = collision.gameObject.GetComponent<PlayerConfig>();
            if(playerConfig != null) {
                playerConfig.setCurrentInteractable(this);
            }
        }
    }

    public new void OnTriggerExit2D(Collider2D collision)
    {
        if (layermask == (layermask | (1 << collision.gameObject.layer))) {
            //disable light
            PlayerConfig playerConfig = collision.gameObject.GetComponent<PlayerConfig>();
            if (playerConfig != null) {
                playerConfig.removeCurrentInteractable(this);
            }
        }
    }

    virtual public void Interact()
    {
        
    }

    virtual public void UnInteract()
    {

    }
}
