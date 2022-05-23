using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrillTrigger : MonoBehaviour
{


    public LayerMask collisionMask;

    public float damagePerTick = 20f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DoDamage(collision);
    }

    private void DoDamage(Collider2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            var prefab = collision.GetComponent<TilePrefab>();
            if (prefab != null)
            {
                prefab.takeDamage(damagePerTick);
            }

            var enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerTick);
            }
        }
    }
}
