using UnityEngine;
using System.Collections;
using PowerTools;

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
	public AnimationClip m_action = null;
	public AnimationClip m_scan = null;
	public AnimationClip m_death = null;
	public AnimationClip m_teleport = null;

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

	public bool scanningAnim = false;

	public float maxDepth = 300f;
	public GameState gameState;


	public Drill drill;
	public Sonar sonar;
	public Vector3 startPos;

	void Start()
	{
		drill = GetComponentInChildren<Drill>();
		sonar = GetComponentInChildren<Sonar>();
		gameState = FindObjectOfType<GameState>();
		controller = GetComponent<ControllerPlayer>();
		m_anim = GetComponent<SpriteAnim>();
		spriteRend = GetComponent<SpriteRenderer>();
	}

    public void ResetPlayer()
    {
		transform.position = startPos;
		gravity = defaultGravity;
		canMove = true;
		drill.canDrill = true;
		sonar.canSonar = true;

	}

    void Update()
	{
		if (canMove) {
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			if(input.magnitude <= 0) {
				if (m_anim.Clip != m_idle) {// (check we're not already in the animation first though)
					m_anim.Play(m_idle);
				}
			} else {
				if (m_anim.Clip != m_walk) { // (check we're not already in the animation first though)
					m_anim.Play(m_walk);
				}
			}

			velocity.x = input.x * moveSpeed;
			velocity.y = gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);

			//Flip character if we need to
			if ((spriteRend.flipX && velocity.x > 0) || (!spriteRend.flipX && velocity.x < 0)) {
				//Looking left moving right OR THE OPPOSITE
				spriteRend.flipX = !spriteRend.flipX;
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


	public float depthAsPercent() {
		return Mathf.Abs(this.transform.position.y) / maxDepth;
    }
}