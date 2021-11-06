using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHelper : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string inSFX = "";
    [FMODUnity.EventRef]
    public string outSFX = "";

    public GameObject canvas;

    // Start is called before the first frame update
    void Awake()
    {
        canvas = this.gameObject;
    }
}
