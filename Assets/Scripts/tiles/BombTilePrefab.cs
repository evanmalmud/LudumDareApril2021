using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTilePrefab : TilePrefab {


    public override void takeDamageVirtual(float damage)
    {
        GetComponent<BombInteractable>().ScanHit();
        //Debug.Log("takeDamage - BombTilePrefab - " + damageUntilDestroyed);
        damageUntilDestroyed -= damage;
        if (damageUntilDestroyed <= 0) {
            Destroy(this.gameObject);
        }
    }

    public override void destroyVirtual()
    {
        Debug.Log("destroyVirtual - BombTilePrefab");
        return;
        //base.destroy();
    }
}
