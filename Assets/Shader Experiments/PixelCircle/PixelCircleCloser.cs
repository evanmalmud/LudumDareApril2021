using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCircleCloser : MonoBehaviour
{

    public string markerName;

    Shapes.Disc disc;

    public float startValue = 4f;

    public float countDownTime = .5f;

    public float currentValue = 0f;

    public bool ran = false;

    private JsonRhythm jsonRhythm;

    private void Awake()
    {
        disc = GetComponent<Shapes.Disc>();
        disc.Radius = 0f;
    }
    public void startTween(JsonRhythm jsonRhythm)
    {
        Debug.Log(markerName + " StartTime " + jsonRhythm.currentUnityTime);
        DOTween.To(x => currentValue = x, startValue, 0, countDownTime).OnComplete(OnComplete).Play();
        ran = true;
        this.jsonRhythm = jsonRhythm;
    }

    // Update is called once per frame
    void Update()
    {
        disc.Radius = currentValue;
    }

    void OnComplete() {
        Debug.Log(markerName + " EndTime " + jsonRhythm.currentUnityTime);
    }
}
