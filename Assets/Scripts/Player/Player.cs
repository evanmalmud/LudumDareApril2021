using UnityEngine;
using System.Collections;
using PowerTools;
using System.Collections.Generic;

[RequireComponent(typeof(ControllerPlayer))]
public class Player : MonoBehaviour {

	public float moveSpeed = 6;
	public float defaultGravity = -20;
	public float gravity = -20;
	public Vector3 velocity;

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



	public AnimationClip m_playerdrillstartidle = null;

	public AnimationClip m_playerdrillidle = null;
	public AnimationClip m_playerdrillwalk = null;

	public AnimationClip m_drillstarting = null;
	public AnimationClip m_drillloop = null;

	[FMODUnity.EventRef]
	public string footstepL = "";
	[FMODUnity.EventRef]
	public string footstepR = "";
	FMOD.Studio.EventInstance footstepLInstance;
	FMOD.Studio.EventInstance footstepRInstance;

	[FMODUnity.EventRef]
	public string deathSfx = "";
	FMOD.Studio.EventInstance deathSfxInstance;

	[FMODUnity.EventRef]
	public string scanSfx = "";
	FMOD.Studio.EventInstance scanSfxInstance;

	[FMODUnity.EventRef]
	public string drillSfx = "";
	FMOD.Studio.EventInstance drillSfxInstance;

	[FMODUnity.EventRef]
	public string teleportSfx = "";
	FMOD.Studio.EventInstance teleportSfxInstance;

	public bool scanningAnim = false;

	public float maxDepth = 300f;
	public GameState gameState;

	bool canDrill = false;
	public bool drillEnabled = false;
	public bool drillEnabledThisFrame = false;

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
		sonar = GetComponentInChildren<Sonar>();
		gameState = FindObjectOfType<GameState>();
		controller = GetComponent<ControllerPlayer>();
		m_anim = GetComponent<SpriteAnim>();
		spriteRend = GetComponent<SpriteRenderer>();
	}

    public void ResetPlayer()
    {
		collectedArtifacts.Clear();
		transform.position = startPos;
		gravity = defaultGravity;
		canMove = true;
		canDrill = true;
		sonar.canSonar = true;
		isDead = false;
		isRecalled = false;
	}

    void Update()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (canMove) {
			velocity.x = input.x * moveSpeed;
			velocity.y = gravity * Time.deltaTime;
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
				drillEnabled = true;
				drillEnabledThisFrame = true;
			} else if (!Input.GetKey(KeyCode.Mouse0)) {
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
		isRecalled = true;
		canMove = false;
		if (m_anim.Clip != m_teleport) { // (check we're not already in the animation first though)
			m_anim.Play(m_teleport);
		}
	}

	public void recallOver() {
		gameState.playerRecalled();	
	}

	public void endScan() {
		canMove = true;
	}

	public void takeDamage() {
		if(!isDead) {
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
		if (!footstepL.Equals(null) && !footstepL.Equals("")) {
			footstepLInstance = FMODUnity.RuntimeManager.CreateInstance(footstepL);
			footstepLInstance.start();
		}
	}

	public void footStepR()
	{
		if (!footstepR.Equals(null) && !footstepR.Equals("")) {
			footstepRInstance = FMODUnity.RuntimeManager.CreateInstance(footstepR);
			footstepRInstance.start();
		}
	}

	public void DeathSfx()
	{
		if (!deathSfx.Equals(null) && !deathSfx.Equals("")) {
			deathSfxInstance = FMODUnity.RuntimeManager.CreateInstance(deathSfx);
			deathSfxInstance.start();
		}
	}

	public void ScanSfx()
	{
		if (!scanSfx.Equals(null) && !scanSfx.Equals("")) {
			scanSfxInstance = FMODUnity.RuntimeManager.CreateInstance(scanSfx);
			scanSfxInstance.start();
		}
	}

	public void DrillSfx()
	{
		if (!drillSfx.Equals(null) && !drillSfx.Equals("")) {
			drillSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillSfx);
			drillSfxInstance.start();
		}
	}

	public void TeleportSfx()
	{
		if (!teleportSfx.Equals(null) && !teleportSfx.Equals("")) {
			teleportSfxInstance = FMODUnity.RuntimeManager.CreateInstance(teleportSfx);
			teleportSfxInstance.start();
		}
	}


	public float depthAsPercent() {
		return Mathf.Abs(this.transform.position.y) / maxDepth;
    }
}