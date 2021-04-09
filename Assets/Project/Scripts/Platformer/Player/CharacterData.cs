using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController2D))]
/// <summary>
/// Used to store an character's attributes
/// </summary>
public class CharacterData : MonoBehaviour {

    // Character's attibrutes
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    public bool isDead;
    [Header("Vertigo")]
    public float vertigoTimeLength = 5f;
    public float vertigoTimeLeft = 0f;
    public float vertigoSpeedreductionPercent = -50f;
    public bool vertigo = false;
    [Header("Intensity")]
    public float intensity = 0f;
    public float intensityDecayPerSecond = 1f;
    public float intensityTimer = 0f;
    [Header("Movement")]
    public float maxSpeedDefault;
    public CharacterFloatStat maxSpeed;
    public float accelerationTime;
    public float decelerationTime;
    public bool canUseSlopesDefault;
    public CharacterBoolStat canUseSlopes;
    [Header("Jumping")]
    public int maxExtraJumps;
    public float maxJumpHeight;
    public float minJumpHeight;
    public bool advancedAirControl;
    public float airAccelerationTime;
    public float airDecelerationTime;
    [Header("Wall Sliding/Jumping")]
    public bool canWallSlide;
    public float wallSlideSpeed;
    public bool canWallJump;
    public float wallJumpSpeed;
    [Header("Dashing")]
    public bool canDash;
    public bool omnidirectionalDash;
    public bool dashDownSlopes;
    public bool canJumpDuringDash;
    public bool jumpCancelStagger;
    public float dashDistance;
    public float dashSpeed;
    public float dashStagger;
    public float maxDashCooldown;
    public int maxAirDashes;
    [Header("Ladders")]
    public float ladderSpeed;
    public float ladderAccelerationTime;
    public float ladderDecelerationTime;
    public float ladderJumpHeight;
    public float ladderJumpSpeed;
    [Header("Color")]
    public Color defaultColor;
    public Color currentColor;
    public Color vertigoColor;


    //Managers
    List<CharacterFloatStat> floats = new List<CharacterFloatStat>();
    List<CharacterBoolStat> bools = new List<CharacterBoolStat>();
    public void Awake()
    {
        maxSpeed = new CharacterFloatStat(maxSpeedDefault);
        floats.Add(maxSpeed);
        canUseSlopes = new CharacterBoolStat(canUseSlopesDefault);
        bools.Add(canUseSlopes);
    }

    public void Update()
    {
       if(vertigo && vertigoTimeLeft > 0) {
            vertigoTimeLeft -= Time.deltaTime;
        } else if(vertigo && vertigoTimeLeft <= 0) {
            vertigo = false;
            disableVertigo();
        }
    }

    public void enableVertigo() {
        CharacterFloatModifier mod = new CharacterFloatModifier("Vertigo",
                CharacterFloatModifier.FloatModifierType.Percent,
                vertigoSpeedreductionPercent, vertigoTimeLength);
        maxSpeed.addModifier(mod);
        currentColor = vertigoColor;
        canUseSlopes.modValue(false);
        vertigoTimeLeft = vertigoTimeLength;
        vertigo = true;
    }

    public void disableVertigo()
    {
        maxSpeed.removeByName("Vertigo");
        currentColor = defaultColor;
        canUseSlopes.modValue(true);
        vertigo = false;
    }
}