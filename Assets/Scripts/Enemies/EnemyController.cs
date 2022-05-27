using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyController
{
    public abstract void ActivateEnemy(Transform target);

    public abstract void TakeDamage(float amount);
}
