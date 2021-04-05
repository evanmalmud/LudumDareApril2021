using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCircleCloser : MonoBehaviour
{

    public string name;

    Material mat;

    public float startValue = .4f;

    public float countDownTime = .5f;

    public float currentValue;

    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }
    public void startTween()
    {
        DOTween.To(x => currentValue = x, startValue, 0, countDownTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (mat.HasProperty("Radius")) {
            mat.SetFloat("Radius", currentValue);
        }

    }
}
