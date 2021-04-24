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

    public LayerMask collisionMask;


    //public CircleCollider2D circleCollider2D;
    public Shapes.Disc disc;

    public Player player;

    public Transform parent;
    public Vector3 localPos;

    public List<GameObject> interactables = new List<GameObject>();

    private void Start()
    {
        player = GetComponentInParent<Player>();
        parent = this.gameObject.transform.parent;
        localPos = this.transform.localPosition;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !sonarEnabled && canSonar) {
            player.scanning(true);
            radius = 0f;
            disc.enabled = true;
            disc.Radius = radius;
            //circleCollider2D.radius = radius;
            currentColor = startColor;
            disc.Color = currentColor;
            sonarEnabled = true;
            this.gameObject.transform.SetParent(null);
            StartCoroutine("scanTime");
        }

        if(sonarEnabled && !sonarStarted) {
            //Kick of doTween
            sonarStarted = true;
            DOTween.To(() => radius, x => radius = x, maxRadius, sonarTime);
            DOTween.To(() => currentColor, x => currentColor = x, endColor, sonarTime).OnComplete(setComplete);
        }

        if(sonarEnabled && sonarStarted) {
            disc.Radius = radius;
            //circleCollider2D.radius = radius;
            disc.Color = currentColor;
        }
    }

    private void setComplete() {
        sonarEnabled = false;
        sonarStarted = false;
        disc.enabled = false;
        this.gameObject.transform.SetParent(parent);
        this.transform.localPosition = localPos;
    }

    IEnumerable scanTime() {
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

        return null;
    }
}
