using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic object that can be active or inactive and affect triggerable objects
/// </summary>
public class TriggerObject : MonoBehaviour {

    public static readonly string ANIMATION_ACTIVE = "active";

    [FMODUnity.EventRef]
    public string eventref = "";

    [Tooltip("If enabled, the trigger will stay active after triggered")]
    public bool oneShot;

    protected Animator animator;

    FMOD.Studio.EventInstance instance;


    /// <summary>
    /// Whether or not the trigger is active
    /// </summary>
    public virtual bool Active { get; protected set; }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        animator = GetComponent<Animator>();
        if(!eventref.Equals("")) {
            instance = FMODUnity.RuntimeManager.CreateInstance(eventref);
        }
    }

    /// <summary>
    /// Toggles the trigger between active and inactive
    /// </summary>
    public virtual void Trigger() {
        instance.start();
        if (!oneShot || !Active) {
            Active = !Active;
            if (animator) {
                animator.SetBool(ANIMATION_ACTIVE, Active);
            }
        }
    }
}