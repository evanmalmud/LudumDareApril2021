using UnityEngine;
using System.Collections;
using PowerTools;
using System.Collections.Generic;
using FMODUnity;

[RequireComponent(typeof(ControllerPlayer))]
public class Player : MonoBehaviour {

	public float moveSpeed = 6;
	public float defaultGravity = -20;
	public float gravity = -20;
	public Vector3 velocity;
    public float jumpVelocity;
	public float timeToJumpApex = .4f;

	public UnityEngine.Rendering.Universal.Light2D playerLight;

	ControllerPlayer controller;

	public float health = 100f;

	public bool isDead = false;

	public bool canMove = false;

	SpriteRenderer spriteRend;
	SpriteAnim m_anim = null;
	public AnimationClip m_idle = null;
	public AnimationClip m_walk = null;
	public AnimationClip m_scan = null;
	public AnimationClip m_death = null;
	public AnimationClip m_teleport = null;

	public AnimationClip m_fall = null;
	public AnimationClip m_land = null;


	public GameObject itemCollect;
	public SpriteAnim itemAnim;
	public SpriteRenderer itemRend;
	public AnimationClip m_itemCollect = null;


	public AnimationClip m_playerdrillstartidle = null;

	public AnimationClip m_playerdrillidle = null;
	public AnimationClip m_playerdrillwalk = null;

	public AnimationClip m_drillstarting = null;
	public AnimationClip m_drillloop = null;

	public EventReference footstepL;
	FMOD.Studio.EventInstance footstepLInstance;

	public EventReference footstepR;
	FMOD.Studio.EventInstance footstepRInstance;

	public EventReference deathSfx;
	FMOD.Studio.EventInstance deathSfxInstance;

	public EventReference scanSfx;
	FMOD.Studio.EventInstance scanSfxInstance;

	public EventReference drillSfx;
	public FMOD.Studio.EventInstance drillSfxInstance;

	public EventReference drillEndSfx;
	FMOD.Studio.EventInstance drillEndSfxInstance;

	public EventReference teleportSfx;
	FMOD.Studio.EventInstance teleportSfxInstance;

	public bool scanningAnim = false;

	public float maxDepth = 100f;
	public GameState gameState;

	public bool canDrill = false;
	public bool drillEnabled = false;
	public bool drillEnabledThisFrame = false;
	public bool drillDisabledThisFrame = false;

	public GameObject drillL;
	public SpriteAnim drillLSpriteAnim;
	public SpriteRenderer drillLSpriteRend;
	public GameObject drillR;
	public SpriteAnim drillRSpriteAnim;
	public SpriteRenderer drillRSpriteRend;
	public Sonar sonar;
	public Vector3 startPos;

	public List<ArtifactScriptableObject> collectedArtifacts = new List<ArtifactScriptableObject>();


	public bool isRecalled = false;
	void Start()
	{
		//Jump
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;


		sonar = GetComponentInChildren<Sonar>();
		gameState = FindObjectOfType<GameState>();
		controller = GetComponent<ControllerPlayer>();
		m_anim = GetComponent<SpriteAnim>();
		spriteRend = GetComponent<SpriteRenderer>();

		if (!teleportSfx.Equals(null) && !teleportSfx.Equals("")) {
			teleportSfxInstance = FMODUnity.RuntimeManager.CreateInstance(teleportSfx);
		}
		if (!footstepL.Equals(null) && !footstepL.Equals("")) {
			footstepLInstance = FMODUnity.RuntimeManager.CreateInstance(footstepL);
		}
		if (!footstepR.Equals(null) && !footstepR.Equals("")) {
			footstepRInstance = FMODUnity.RuntimeManager.CreateInstance(footstepR);
		}
		if (!deathSfx.Equals(null) && !deathSfx.Equals("")) {
			deathSfxInstance = FMODUnity.RuntimeManager.CreateInstance(deathSfx);
		}
		if (!scanSfx.Equals(null) && !scanSfx.Equals("")) {
			scanSfxInstance = FMODUnity.RuntimeManager.CreateInstance(scanSfx);
		}
		if (!drillSfx.Equals(null) && !drillSfx.Equals("")) {
			drillSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillSfx);
		}
		if (!drillEndSfx.Equals(null) && !drillEndSfx.Equals("")) {
			drillEndSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillEndSfx);
		}
		canMove = false;
		canDrill = false;
		//sonar.canSonar = false;
		drillEnabled = false;
		drillL.SetActive(drillEnabled);
		drillR.SetActive(drillEnabled);
		this.gameObject.SetActive(false);
	}

	public void OnDestroy()
	{
		footstepLInstance.release();
		footstepRInstance.release();
		deathSfxInstance.release();
		scanSfxInstance.release();
		drillSfxInstance.release();
		drillEndSfxInstance.release();
		teleportSfxInstance.release();
	}

	public void ResetPlayer()
    {
		this.gameObject.SetActive(true);
		enableLight();
		collectedArtifacts.Clear();
		transform.position = startPos;
		gravity = defaultGravity;
		canMove = true;
		canDrill = true;
		//sonar.canSonar = true;
		isDead = false;
		isRecalled = false;
		drillL.SetActive(drillEnabled);
		drillR.SetActive(!drillEnabled);
	}

    void FixedUpdate()
	{
		if(isDead) {
			canDrill = false;
			canMove = false;
			drillEnabled = false;
		}
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		bool jumpinput = false;//Input.GetKeyDown(KeyCode.Space);
		if (canMove) {
			if (jumpinput && controller.isGrounded) {
				Debug.Log("Jump added");
				velocity.y = jumpVelocity;
			} else {
				velocity.y = gravity * Time.deltaTime;
			}
			velocity.x = input.x * moveSpeed;
			controller.Move(velocity * Time.deltaTime);

			//Flip character if we need to
			if ((spriteRend.flipX && velocity.x > 0) || (!spriteRend.flipX && velocity.x < 0)) {
				//Looking left moving right OR THE OPPOSITE
				spriteRend.flipX = !spriteRend.flipX;
			}
		}

		if(canDrill) {
			drillEnabledThisFrame = false;
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				DrillSfx();
				drillEnabled = true;
				drillEnabledThisFrame = true;
			} else if (!Input.GetKey(KeyCode.Mouse0)) {
				if(drillEnabled) {
					drillSfxInstance.setPaused(true);
					DrillEndSfx();
				}
				drillEnabled = false;
				drillL.SetActive(drillEnabled);
				drillR.SetActive(drillEnabled);
			}
			if (drillEnabled) {

				//rotation
				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 0f;

				Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
				mousePos.x = mousePos.x - objectPos.x;
				mousePos.y = mousePos.y - objectPos.y;

				float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

				if (spriteRend.flipX) {
					//Facing Left
					drillL.SetActive(drillEnabled);
					drillR.SetActive(!drillEnabled);
					if (angle < 90 && angle >= 0) {
						angle = 90f;
					} else if (angle > -90 && angle <= 0) {
						angle = -90f;
					}

					drillL.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
				} else {
					drillL.SetActive(!drillEnabled);
					drillR.SetActive(drillEnabled);
					if (angle > 90) {
						angle = 90f;
					} else if (angle < -90) {
						angle = -90f;
					}
					drillR.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
				}
			}
		}

		// Animations
		if (drillEnabled) {
			if(drillEnabledThisFrame) {
				
				if (drillRSpriteAnim.Clip != m_drillstarting && drillR.activeSelf) {
					drillRSpriteAnim.Play(m_drillstarting);
				}
				if (drillLSpriteAnim.Clip != m_drillstarting && drillL.activeSelf) {
					drillLSpriteAnim.Play(m_drillstarting);
				}
			} else {
				if (drillRSpriteAnim.Clip != m_drillloop && drillR.activeSelf) {
					drillRSpriteAnim.Play(m_drillloop);
				}
				if (drillLSpriteAnim.Clip != m_drillloop && drillL.activeSelf) {
					drillLSpriteAnim.Play(m_drillloop);
				}
			}
			if (canMove && input.magnitude > 0) {
				//Moving and Drilling
				if (m_anim.Clip != m_playerdrillwalk) {// (check we're not already in the animation first though)
					m_anim.Play(m_playerdrillwalk);
				}
			} else if (canMove) {
				//Not moving
				if (drillEnabledThisFrame) {
					if (m_anim.Clip != m_playerdrillstartidle) {// (check we're not already in the animation first though)
						m_anim.Play(m_playerdrillstartidle);
					}
				} else {
					if (m_anim.Clip != m_playerdrillidle) {// (check we're not already in the animation first though)
						m_anim.Play(m_playerdrillidle);
					}
				}
			}

		} else if (canMove) {
			if (input.magnitude > 0) {
				//Moving
				if (m_anim.Clip != m_walk) {// (check we're not already in the animation first though)
					m_anim.Play(m_walk);
				}
			} else {
				if (m_anim.Clip != m_idle) {// (check we're not already in the animation first though)
					m_anim.Play(m_idle);
				}
			}
		}
	}

	public void scanning(bool isScanning) {
		canMove = false;
		if (m_anim.Clip != m_scan) { // (check we're not already in the animation first though)
			m_anim.Play(m_scan);
		}
	}

	public void playRecall() {
		if (!isDead && !isRecalled) {
			isRecalled = true;
			canMove = false;
			if (m_anim.Clip != m_teleport) { // (check we're not already in the animation first though)
				m_anim.Play(m_teleport);
			}
		}
	}

	public void endScan() {
		canMove = true;
	}

	public void takeDamage() {
		Debug.Log("takeDamage called");
		if(!isDead && !isRecalled) {
			isDead = true;
			canMove = false;
			if (m_anim.Clip != m_death) { // (check we're not already in the animation first though)
				m_anim.Play(m_death);
			}
			gravity = 0f;
			gameState.playerDied();
		}

	}

	public void footStepL() {
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

	public void ScanSfx()
	{
		scanSfxInstance.start();
	}

	public void DrillSfx()
	{
		drillSfxInstance.setPaused(false);
		drillSfxInstance.start();
	}

	public void DrillEndSfx()
	{
		drillEndSfxInstance.start();
	}


	public void TeleportSfx()
	{
		teleportSfxInstance.start();
	}


	public float depthAsPercent() {
		return Mathf.Abs(this.transform.position.y) / maxDepth;
    }

	public void disableLight() {
		playerLight.enabled = false;
	}

	public void enableLight()
	{
		playerLight.enabled = true;
	}

}