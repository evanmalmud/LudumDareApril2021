using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(BoxCollider2D))]
public class Box2DToggle : MonoBehaviour
{
    public Light2D light2d;

    public LayerMask layermask = 0;

    BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        if (light2d != null)
            light2d.enabled = false;
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (layermask == (layermask | (1 << collision.gameObject.layer))) {
            EnableLight();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (layermask == (layermask | (1 << collision.gameObject.layer))) {
            DisableLight();
        }
    }

    public void EnableLight() {
        if (light2d != null)
            light2d.enabled = true;
    }

    public void DisableLight() {
        if (light2d != null)
            light2d.enabled = false;
    }
}
