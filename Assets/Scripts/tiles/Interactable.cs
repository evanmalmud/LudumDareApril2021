using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour {

    public SpriteRenderer spriteRend;
    
    public UnityEngine.Rendering.Universal.Light2D light2d;

    public virtual void Start()
    {
        if(spriteRend == null) {
            spriteRend = GetComponent<SpriteRenderer>();
        }
        spriteRend.sortingLayerName = "BG";
        light2d = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        light2d.enabled = false;
    }

    public virtual void ScanHit() {
        spriteRend.sortingLayerName = "FX";
        light2d.enabled = true;
    }


}
