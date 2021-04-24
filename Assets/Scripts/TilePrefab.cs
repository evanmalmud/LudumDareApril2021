using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefab : MonoBehaviour
{

    public float damageUntilDestroyed = 100f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage) {
        damageUntilDestroyed -= damage;
        if(damageUntilDestroyed <= 0) {
            Destroy(this.gameObject);
        }
    }
}
