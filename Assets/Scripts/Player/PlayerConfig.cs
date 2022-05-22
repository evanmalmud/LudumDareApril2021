using FMODUnity;
using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour {// movement config\

    public PlayerState playerState;

    public PlayerState defaultState;

    public SceneLoader _sceneLoader;
    public List<ArtifactScriptableObject> collectedArtifacts = new List<ArtifactScriptableObject>();
    public List<OreScriptableObject> collectedOres = new List<OreScriptableObject>();
    public CollectEffect collectEffect;

    public int maxDepth = 300;

    private SpriteAnim _spriteAnim;

    //Player Animation State
    public AnimationClip player_idle;
    public AnimationClip player_walk;
    public AnimationClip player_land;
    public AnimationClip player_fall;
    public AnimationClip player_jump;
    public AnimationClip player_dead;
    public AnimationClip player_scan;
    public AnimationClip player_tele;

    [HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private PrimeCharacterController _controller;

	private Drill _drill;

    private Sonar _sonar;

 
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	public PlayerInteractable currentInteractable;
	public ShipInteriorManager shipInteriorManager;

    public EventReference footstepL;
    FMOD.Studio.EventInstance footstepLInstance;

    public EventReference footstepR;
    FMOD.Studio.EventInstance footstepRInstance;

    public EventReference teleportSfx;
    FMOD.Studio.EventInstance teleportSfxInstance;

    public EventReference deathSfx;
    FMOD.Studio.EventInstance deathSfxInstance;

    public PauseController pauseController;

    void Awake()
	{
        pauseController = FindObjectOfType<PauseController>();
        _spriteAnim = GetComponent<SpriteAnim>();
        _controller = GetComponent<PrimeCharacterController>();
        _drill = GetComponent<Drill>();
        _sonar = GetComponent<Sonar>();
        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

    }

    private void Start()
    {
        if (!footstepL.Equals(null) && !footstepL.Equals("")) {
            footstepLInstance = FMODUnity.RuntimeManager.CreateInstance(footstepL);
        }
        if (!footstepR.Equals(null) && !footstepR.Equals("")) {
            footstepRInstance = FMODUnity.RuntimeManager.CreateInstance(footstepR);
        }
        if (!deathSfx.Equals(null) && !deathSfx.Equals("")) {
            deathSfxInstance = FMODUnity.RuntimeManager.CreateInstance(deathSfx);
        }
        if (!teleportSfx.Equals(null) && !teleportSfx.Equals("")) {
            teleportSfxInstance = FMODUnity.RuntimeManager.CreateInstance(teleportSfx);
        }
    }

    public void PausedState(bool enabled) {
        if (enabled) {
            //Enabling Pause
            playerState.canMove = false;
            playerState.canDrill = false;
            playerState.canSonar = false;
        } else {
            //Disabling pause
            playerState.canMove = defaultState.canMove;
            playerState.canDrill = defaultState.canDrill;
            playerState.canSonar = defaultState.canSonar;
        }
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
	{
		// bail out on plain old ground hits cause they arent very interesting
		if (hit.normal.y == 1f)
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent(Collider2D col)
	{
		//Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
	}


	void onTriggerExitEvent(Collider2D col)
	{
		//Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
	}

    #endregion


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(currentInteractable == null || 
                !currentInteractable.interactActive) {
                pauseController.PauseInteractionToggle();
            }
        }


            //Dont want to move when sonarAnimActive
        if (playerState.canMove && !_sonar.sonarAnimActive) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                //Debug.Log("Escape Pressed");
                shipInteriorManager.ZoomCameraToPlayer();

                if (currentInteractable != null) {
                    //Debug.Log("Interactable Not Null");
                    currentInteractable.UnInteract();
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                //Debug.Log("E Pressed");
                if (currentInteractable != null) {
                    //Debug.Log("Interactable Not Null");
                    currentInteractable.Interact();
                }
            }

            if (_controller.isGrounded) {
                _velocity.y = 0;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            } else {
                normalizedHorizontalSpeed = 0;
            }


            // we can only jump whilst grounded
            if (_controller.isGrounded && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))) {
                _velocity.y = Mathf.Sqrt(2f * playerState.jumpHeight * -playerState.gravity);
            }


            // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
            var smoothedMovementFactor = _controller.isGrounded ? playerState.groundDamping : playerState.inAirDamping; // how fast do we change direction?
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * playerState.runSpeed, Time.deltaTime * smoothedMovementFactor);

            // apply gravity before moving
            _velocity.y += playerState.gravity * Time.deltaTime;

            // if holding down bump up our movement amount and turn off one way platform detection for a frame.
            // this lets us jump down through one way platforms
            //if (_controller.isGrounded && Input.GetKey(KeyCode.DownArrow)) {
            //_velocity.y *= 3f;
            //EDIT: Edit to Prime31 Code to fix on-way platform drop through
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                _controller.ignoreOneWayPlatformsThisFrame = true;
            }

            _controller.move(_velocity * Time.deltaTime);

            // grab our current _velocity to use as a base for all calculations
            _velocity = _controller.velocity;
        } else {
            //Still want to fall if dead or recalled
            // apply gravity before moving
            _velocity.y += playerState.gravity * Time.deltaTime;
            _controller.move(_velocity * Time.deltaTime);
            // grab our current _velocity to use as a base for all calculations
            _velocity = _controller.velocity;
        }

        //Drill (Dont want to drill when sonar active)
        if (playerState.canDrill && !_sonar.sonarAnimActive) {
            bool mousePressedDown = Input.GetKeyDown(KeyCode.Mouse0);
            bool mousePressedHeld = Input.GetKey(KeyCode.Mouse0);
            bool playerDirectionLeft = transform.localScale.x < 0;
            //Debug.Log("mousePressedDown - " + mousePressedDown);
            //Debug.Log("mousePressedHeld - " + mousePressedHeld);
            //Debug.Log("playerDirectionLeft - " + playerDirectionLeft);
            _drill.drillUpdate(mousePressedDown, mousePressedHeld, playerDirectionLeft);
        } else {
            _drill.drillUpdate(false, false, false);
        }

        //Sonar
        if (playerState.canSonar) {
            bool ePressed = Input.GetKeyDown(KeyCode.E);
            _sonar.SonarUpdate(ePressed);
            //_playerState.playAnim(_playerState.player_scan);
        }

        UpdateAnimation();
    }



	public void setCurrentInteractable(PlayerInteractable interactable) {
		if (currentInteractable != interactable) {
			if (currentInteractable != null) {
				currentInteractable.DisableLight();
			}
			interactable.EnableLight();
			currentInteractable = interactable;
		}
	}

	public void removeCurrentInteractable(PlayerInteractable interactable)
	{
        interactable.UnInteract();
		interactable.DisableLight();
		if(currentInteractable == interactable) {
			currentInteractable = null;
        }
	}

	public PlayerInteractable getCurrentInteractable()
	{
		return currentInteractable;
	}


    public void takeDamage()
    {
        if (!playerState.isDead && !playerState.isRecalled) {
            playerState.isDead = true;
            playerState.canMove = false;
            playerState.canDrill = false;
            playerState.canSonar = false;
            DeathSfx();

            //Trigger scene reload
            _sceneLoader.LoadScene();
        }
    }

    public void Recall()
    {
        if (!playerState.isDead && !playerState.isRecalled) {
            playerState.isRecalled = true;
            playerState.canMove = false;
            playerState.canDrill = false;
            playerState.canSonar = false;
            TeleportSfx();
        }
    }

    public void UpdateAnimation()
    {
        if (playerState.isDead) {
            playAnim(player_dead);
        } else if (playerState.isRecalled) {
            playAnim(player_tele);
        } else if (_sonar.sonarAnimActive) {
            playAnim(player_scan);
        } else {
            if (_controller.isGrounded) {
                if (normalizedHorizontalSpeed == 0 && Mathf.Abs(_velocity.x) < .02f) {
                    playAnim(player_idle);
                } else {
                    playAnim(player_walk);
                }
            } else {
                playAnim(player_jump);
            }
        }

    }

    public void playAnim(AnimationClip anim)
    {
        if (_spriteAnim.Clip != anim) {// (check we're not already in the animation first though)
            _spriteAnim.Play(anim);
        }
    }


    public void collectOre(OreScriptableObject ore) {
        collectedOres.Add(ore);
        playerState.moneyScore += ore.value;
        triggerCollectEffect();
    }

    public void collectArtifact(ArtifactScriptableObject artifact)
    {
        collectedArtifacts.Add(artifact);
        triggerCollectEffect();
    }

    public void triggerCollectEffect() {
        collectEffect.collectAnim();
    }

    public void footStepL()
    {
        footstepLInstance.start();
    }

    public void footStepR()
    {
        footstepRInstance.start();
    }

    public void DeathSfx()
    {
        deathSfxInstance.start();
    }

    public void TeleportSfx()
    {
        teleportSfxInstance.start();
    }

    public void OnDestroy()
    {
        footstepLInstance.release();
        footstepRInstance.release();
        deathSfxInstance.release();
        teleportSfxInstance.release();
    }

    public float depthAsPercent()
    {
        return Mathf.Abs(this.transform.position.y) / maxDepth;
    }

}