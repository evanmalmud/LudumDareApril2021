using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerPassed : MonoBehaviour
{

    public TilemapPrefabLoader loader;

    public LayerMask collisionMask;

    private void Start()
    {
        loader = FindObjectOfType<TilemapPrefabLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer))) {
            //Debug.Log("load layer");
            loader.logLayerPassed();
            Destroy(this.gameObject);
        }
    }
}
