using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscTween : MonoBehaviour
{

    public Shapes.Disc disc;

    public float currentValue = 0f;

    public RhythmInput rhythmInput;

    public float preInputTime = .1f;

    public float postInputTime = .1f;

    public string keyName;

    public float currentTime = 0f;

    public bool expectingButton = false;

    public float startValue;

    public float countDownTime;

    // Start is called before the first frame update
    void Awake()
    {
        disc = GetComponent<Shapes.Disc>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= countDownTime + postInputTime) {
            tweenCompleted();
            return;
        } else {
            currentTime += Time.deltaTime;
        }
        if (currentTime < countDownTime) {
            currentValue =  Mathf.Lerp(startValue, 0f, currentTime/countDownTime);
            disc.Radius = currentValue;
        }
        if (!expectingButton && currentTime < countDownTime - preInputTime) {
            rhythmInput.expectedButtons.Add(keyName);
            expectingButton = true;
        }
    }

    public void startTween(float startValue, float countDownTime, RhythmInput rhythmInput, float preInputTime, float postInputTime, string keyName)
    {
        Debug.Log("startTween " + keyName + " " + Time.time);
        this.startValue = startValue;
        this.preInputTime = preInputTime;
        this.postInputTime = postInputTime;
        this.keyName = keyName;
        this.rhythmInput = rhythmInput;
        this.currentValue = startValue;
        this.currentTime = 0f;
        this.countDownTime = countDownTime;
        disc.Radius = startValue;
    }

    public void tweenCompleted() {
        rhythmInput.expectedButtons.Remove(keyName);
        Debug.Log("Removing " + keyName + " " + Time.time);
        Destroy(this.gameObject);
    }
}
