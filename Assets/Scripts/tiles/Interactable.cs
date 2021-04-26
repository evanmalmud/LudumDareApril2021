using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Interactable : MonoBehaviour {

    public SpriteRenderer spriteRend;
    
    public Light2D light2d;

    public virtual void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sortingLayerName = "BG";
        light2d = GetComponent<Light2D>();
        light2d.enabled = false;
    }

    public virtual void ScanHit() {
        spriteRend.sortingLayerName = "FX";
        light2d.enabled = true;
    }


}
