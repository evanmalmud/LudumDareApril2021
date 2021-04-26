using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenShake : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Duration of the screen shake.")]
    [SerializeField] private float shakeDuration = .25f;
    [Tooltip("Amplitude of the screen shake.")]
    [SerializeField] private float shakeAmount = 0.25f;
    [Tooltip("Factor by which the amplitude is decreased each update.")]
    [SerializeField] private float decreaseFactor = 4.0f;
    [Header("Test")]
    [Tooltip("Set this boolean to test the screen shake. Only works in Play Mode.")]
    [SerializeField] private bool testScreenShake;

    private Transform camTransform;

    Vector3 originalPos;

    void Awake()
    {
        camTransform = GetComponent<Transform>();
    }

    void Start()
    {
        originalPos = camTransform.localPosition;
    }
    /// <summary>
    /// If called the screen will shake with the preset defaults.
    /// </summary>
    public void ShakeScreenDefault()
    {
        originalPos = camTransform.localPosition;
        StartCoroutine(ScreenShakeCoroutine(shakeDuration, shakeAmount, decreaseFactor));
    }

    /// <summary>
    /// Screen shake with custom Settings
    /// </summary>
    /// <param name="shakeDuration"></param>
    /// <param name="shakeAmount"></param>
    /// <param name="decreaseFactor"></param>
    public void ShakeScreenCustom(float shakeDuration, float shakeAmount, float decreaseFactor)
    {
        originalPos = camTransform.localPosition;
        StartCoroutine(ScreenShakeCoroutine(shakeDuration, shakeAmount, decreaseFactor));
    }

    void Update()
    {
        if (testScreenShake) {
            testScreenShake = false;
            originalPos = camTransform.localPosition;
            StartCoroutine(ScreenShakeCoroutine(shakeDuration, shakeAmount, decreaseFactor));
        }
    }

    IEnumerator ScreenShakeCoroutine(float shakeDuration, float shakeAmount, float decreaseFactor)
    {
        float countdown = shakeDuration;
        while (countdown > 0) {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            countdown -= Time.deltaTime * decreaseFactor;
            yield return new WaitForEndOfFrame();
        }
        camTransform.localPosition = originalPos;
    }
}
