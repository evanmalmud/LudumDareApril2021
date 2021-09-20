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

    public LayerMask collisionMask;


    public CircleCollider2D circleCollider2D;
    public Shapes.Disc disc;

    public Transform parent;
    public Vector3 localPos;

    public List<GameObject> interactables = new List<GameObject>();

    [FMODUnity.EventRef]
    public string scanSfx = "";
    public FMOD.Studio.EventInstance scanSfxInstance;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        parent = this.gameObject.transform.parent;
        localPos = this.transform.localPosition;
    }

    public void SonarUpdate(bool buttonPressed) {
        if (buttonPressed && !sonarEnabled) {
            radius = 0f;
            disc.enabled = true;
            circleCollider2D.enabled = true;
            disc.Radius = radius;
            circleCollider2D.radius = radius;
            currentColor = startColor;
            disc.Color = currentColor;
            sonarEnabled = true;
            this.gameObject.transform.SetParent(null);
            // IEnumerator couroutine = ScanTime();
            //StartCoroutine(couroutine);
        }
    }


    private void Update()
    {
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
        circleCollider2D.enabled = false;
        this.gameObject.transform.SetParent(parent);
        this.transform.localPosition = localPos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interact;
        if (collision.TryGetComponent<Interactable>(out interact)) {
            interact.ScanHit();
        }
    }

    IEnumerator ScanTime() {
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(this.transform.position, maxRadius, collisionMask);
        foreach(Collider2D hit in hitCollider) {
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
