using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public float radius = 0f;

    public float maxRadius = 500f;

    public float sonarTime = 5f;

    public Color startColor = new Color32(0x30, 0xE1, 0xB9, 0xFF);
    public Color endColor = new Color32(0x0B, 0x8A, 0x8F, 0xFF);
    public Color currentColor;
    public bool sonarEnabled = false;

    public bool sonarStarted = false;

    public bool canSonar = false;


    public CircleCollider2D circleCollider2D;
    public Shapes.Disc disc;

    private void Start()
    {
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !sonarEnabled && canSonar) {
            radius = 0f;
            disc.enabled = true;
            disc.Radius = radius;
            circleCollider2D.radius = radius;
            currentColor = startColor;
            disc.Color = currentColor;
            sonarEnabled = true;
        }

        if(sonarEnabled && !sonarStarted) {
            //Kick of doTween
            sonarStarted = true;
            DOTween.To(() => radius, x => radius = x, maxRadius, sonarTime);
            DOTween.To(() => currentColor, x => currentColor = x, endColor, sonarTime).OnComplete(setComplete);
        }

        if(sonarEnabled && sonarStarted) {
            disc.Radius = radius;
            circleCollider2D.radius = radius;
            disc.Color = currentColor;
        }
    }

    private void setComplete() {
        sonarEnabled = false;
        sonarStarted = false;
        disc.enabled = false;
    }
}
