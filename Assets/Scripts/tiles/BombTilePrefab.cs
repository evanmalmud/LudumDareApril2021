using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTilePrefab : TilePrefab {


    public override void takeDamage(float damage)
    {
        GetComponent<BombInteractable>().ScanHit();
        damageUntilDestroyed -= damage;
        if (damageUntilDestroyed <= 0) {
            
            Destroy(this.gameObject);
        }
    }
}
