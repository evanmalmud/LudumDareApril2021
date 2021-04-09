using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RhythmStarter : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!", this);

        // invoke your event
        FindObjectOfType<JsonRhythm>().startMusicbool = true;
        Destroy(this.gameObject);
    }
}
