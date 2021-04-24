using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ControllerPlayer))]
public class Player : MonoBehaviour {

	public float moveSpeed = 6;
	public float gravity = -20;
	public Vector3 velocity;

	ControllerPlayer controller;

	void Start()
	{
		controller = GetComponent<ControllerPlayer>();
	}

	void Update()
	{

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}