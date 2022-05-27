using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour, EnemyController
{


    public AnimationClip m_death;
    public AnimationClip m_idle;
    public AnimationClip m_singleShot;
    public AnimationClip m_DoubleShot;

    public enum TURRET_STATE
    {
        IDLE, //Perched
        SINGLE,
        DOUBLE,
        DEATH
    }

    public void ActivateEnemy(Transform target)
    {
        
    }

    public void TakeDamage(float amount)
    {
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
