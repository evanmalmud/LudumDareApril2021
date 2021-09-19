using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
	public float health = 100f;
	public bool isDead = false;
    public bool isRecalled = false;
	public bool canMove = false;
	public bool canSonar = false;
	public bool canDrill = false;

    public void Start()
    {
        canMove = true;
        canDrill = true;
        isDead = false;
        isRecalled = false;
    }

    public void Reset()
    {
        canMove = true;
        canDrill = true;
        isDead = false;
        isRecalled = false;
    }
}
