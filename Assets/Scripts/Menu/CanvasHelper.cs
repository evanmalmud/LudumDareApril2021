using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHelper : MonoBehaviour
{

    public EventReference inSFX;

    public EventReference outSFX;

    public GameObject canvas;

    // Start is called before the first frame update
    void Awake()
    {
        canvas = this.gameObject;
    }
}
