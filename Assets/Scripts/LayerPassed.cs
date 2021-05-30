using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerPassed : MonoBehaviour
{

    public TilemapPrefabLoader loader;

    public LayerMask collisionMask;

    public BoxCollider2D collider;

    private void Awake()
    {
        loader = FindObjectOfType<TilemapPrefabLoader>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer))) {
            //Debug.Log("load layer");
            loader.logLayerPassed();
            collider.enabled = false;
        }
    }
}
