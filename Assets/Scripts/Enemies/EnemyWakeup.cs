using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWakeup : MonoBehaviour
{

    public List<GameObject> enemies;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

            EnemyTargetTracker enemyTargetTracker = collision.gameObject.GetComponent<EnemyTargetTracker>();
            foreach (GameObject go in enemies)
            {
                EnemyController enemyController = go.GetComponent<EnemyController>();
                if(enemyController != null)
                {
                    enemyController.ActivateEnemy(enemyTargetTracker.getTarget());
                }
            }

            //Disable this
            gameObject.SetActive(false);
        }
    }
}
