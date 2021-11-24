using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarScan : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public Shapes.Disc disc;

    public float radius = 0f;

    public float maxRadius = 500f;

    public float sonarTime = 5f;

    public Color startColor = new Color32(0x30, 0xE1, 0xB9, 0xFF);
    public Color endColor = new Color32(0x0B, 0x8A, 0x8F, 0xFF);
    public Color currentColor;
    public LayerMask collisionMask;

    Sonar creatorObj;

    [FMODUnity.EventRef]
    public string scanSfx = "";
    public FMOD.Studio.EventInstance scanSfxInstance;

    // Start is called before the first frame update
    public void StartScan(Sonar creatorObj)
    {
        this.creatorObj = creatorObj;
        disc.enabled = true;
        circleCollider2D.enabled = true;
        disc.Radius = radius;
        circleCollider2D.radius = radius;
        currentColor = startColor;
        disc.Color = currentColor;

        DOTween.To(() => radius, x => radius = x, maxRadius, sonarTime);
        DOTween.To(() => currentColor, x => currentColor = x, endColor, sonarTime).OnComplete(setComplete);
    }

    // Update is called once per frame
    void Update()
    {
        disc.Radius = radius;
        circleCollider2D.radius = radius;
        disc.Color = currentColor;
    }

    private void setComplete()
    {
        disc.enabled = false;
        circleCollider2D.enabled = false;
        creatorObj.SonarComplete();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interact;
        if (collision.TryGetComponent<Interactable>(out interact)) {
            interact.ScanHit();
        }
    }

    IEnumerator ScanTime()
    {
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(this.transform.position, maxRadius, collisionMask);
        foreach (Collider2D hit in hitCollider) {
            Interactable interact;
            if (hit.TryGetComponent<Interactable>(out interact)) {
                interact.ScanHit();
            }
            // BombInteractable bomb;
            //if (hit.TryGetComponent<BombInteractable>(out bomb)) {
            //    bomb.ScanHit();
            //}
        }

        yield return null;
    }
}
