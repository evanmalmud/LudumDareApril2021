using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsConfig : MonoBehaviour {
    [Tooltip("Which layers are considered ground")]
    public LayerMask groundMask;
    [Tooltip("Which layers are considered one way platforms")]
    public LayerMask owPlatformMask;
    [Tooltip("Which layers are considered passengers for platforms")]
    public LayerMask passengerMask;
    [Tooltip("Which layers are considered ladders")]
    public LayerMask ladderMask;
    [Tooltip("Which layers are considered characters")]
    public LayerMask characterMask;
    [Tooltip("Which layers characters can collide with")]
    public LayerMask characterCollisionMask;
    [Tooltip("Which layers stand-on objects will move")]
    public LayerMask standOnCollisionMask;
    [Tooltip("Which layers are considered interactable objects")]
    public LayerMask interactableMask;
    public float gravity = -30f;
    public float airFriction = 15f;
    public float groundFriction = 30f;
    public float staggerSpeedFalloff = 50f;

}