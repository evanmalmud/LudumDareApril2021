using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTilePrefab : TilePrefab {


    public override void takeDamageVirtual(float damage)
    {
        GetComponent<BombInteractable>().ScanHit();
        //Debug.Log("takeDamage - BombTilePrefab - " + damageUntilDestroyed);
        currentDamageUntilDestroyed -= damage;
        if (currentDamageUntilDestroyed <= 0) {
            Destroy(this.gameObject);
        }
    }

    public override void destroyVirtual()
    {
       // Debug.Log("destroyVirtual - BombTilePrefab");
        BombInteractable bombInt = GetComponent<BombInteractable>();
        if(bombInt != null) {
            if(!bombInt.bombTriggered) {
                Destroy(this.gameObject);
            }
        }
        return;
    }
}
