using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;
    public bool canMove = true;
    public bool canDrill = true;
    public bool canSonar = true;
    public bool isDead = false;
    public bool isRecalled = false;

    public bool isShieldActive = false;

    public int moneyScore = 0;

}
