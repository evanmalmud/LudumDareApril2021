using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFMODTHING : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string TESTevent = "";


    public void PlayTestEvent() {
        Debug.Log("PlayTestEvent Called");
        FMODUnity.RuntimeManager.PlayOneShot(TESTevent, transform.position);
    }
}
