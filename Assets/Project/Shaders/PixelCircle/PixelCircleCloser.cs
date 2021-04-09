using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCircleCloser : MonoBehaviour
{

    public string markerName;

    public string keyName;

    public GameObject shapePrefab;

    public float startValue = 4f;

    public float countDownTime = .5f;

    public float preInputTime = .1f;

    public float postInputTime = .1f;

    public float videoaudiobuffer = .1f;

    public void startTween(RhythmInput rhythmInput)
    {
        GameObject go = Instantiate(shapePrefab, transform);
        go.GetComponent<DiscTween>().startTween(startValue, countDownTime + videoaudiobuffer, rhythmInput, preInputTime, postInputTime, keyName);
    }

    public void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, startValue);
    }
}
