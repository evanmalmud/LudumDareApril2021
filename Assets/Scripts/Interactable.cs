using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Interactable : MonoBehaviour {


    public Light2D light2d;

    public void Start()
    {
        light2d = GetComponent<Light2D>();
        light2d.enabled = false;
    }

    public virtual void ScanHit() {
        light2d.enabled = true;
    }


}
