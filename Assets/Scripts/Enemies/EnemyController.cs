using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyController
{
    public abstract void ActivateEnemy(GameObject target);

    public abstract void TakeDamage(float amount);
}
