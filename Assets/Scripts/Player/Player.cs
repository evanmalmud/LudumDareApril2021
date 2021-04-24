using UnityEngine;
using System.Collections;
using PowerTools;

[RequireComponent(typeof(ControllerPlayer))]
public class Player : MonoBehaviour {

	public float moveSpeed = 6;
	public float gravity = -20;
	public Vector3 velocity;

	ControllerPlayer controller;

	public float health = 100f;

	public bool canMove = false;

	SpriteRenderer spriteRend;
	SpriteAnim m_anim = null;
	public AnimationClip m_idle = null;
	public AnimationClip m_walk = null;
	public AnimationClip m_action = null;
	public AnimationClip m_scan = null;
	public AnimationClip m_death = null;

	public bool scanningAnim = false;

	void Start()
	{
		controller = GetComponent<ControllerPlayer>();
		m_anim = GetComponent<SpriteAnim>();
		spriteRend = GetComponent<SpriteRenderer>();
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

	public void endScan() {
		canMove = true;
	}

	public void takeDamage() {
		
    }
}