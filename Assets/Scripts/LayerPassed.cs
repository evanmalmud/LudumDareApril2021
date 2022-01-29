using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerPassed : MonoBehaviour
{

    public TilemapPrefabLoader loader;

    public LayerMask collisionMask;

    public BoxCollider2D collider2d;

    private void Awake()
    {
        loader = FindObjectOfType<TilemapPrefabLoader>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        collider2d.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter2D layer - " + collision.gameObject.name);
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer))) {
           // Debug.Log("load layer");
            loader.logLayerPassed();
            collider2d.enabled = false;
        }
    }
}
