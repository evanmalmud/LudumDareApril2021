using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrillTrigger : MonoBehaviour
{


    public LayerMask collisionMask;

    public float damagePerTick = 20f;

    void Start()
    {

    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnCollisionEnter2D collision");
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer))) {
            //Delete Tile
            Debug.Log("OnCollisionEnter2D delete " + collision.gameObject.name);
            TilePrefab prefab = collision.GetComponent<TilePrefab>();
            if (prefab != null) {
                prefab.takeDamage(damagePerTick);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("OnCollisionEnter2D collision");
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer))) {
            //Delete Tile
            Debug.Log("OnCollisionEnter2D delete " + collision.gameObject.name);
            TilePrefab prefab = collision.GetComponent<TilePrefab>();
            if (prefab != null) {
                prefab.takeDamage(damagePerTick);
            }
        }
    }
}
