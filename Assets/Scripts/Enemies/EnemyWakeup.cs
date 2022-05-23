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
            foreach(GameObject go in enemies)
            {
                EnemyController enemyController = go.GetComponent<EnemyController>();
                if(enemyController != null)
                {
                    enemyController.ActivateEnemy(collision.gameObject);
                }
            }

            //Disable this
            gameObject.SetActive(false);
        }
    }
}
