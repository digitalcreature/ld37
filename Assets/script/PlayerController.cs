using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 5;
	public float sensitivity = 5;

	Camera camera;
	CharacterController controller;

	void Awake() {
		controller = GetComponent<CharacterController>();
		camera = GetComponentInChildren<Camera>();
		controller.slopeLimit = 90;
		controller.stepOffset = 0;
	}

	void Update() {
		Vector3 move = new Vector3(
			Input.GetAxisRaw("Horizontal"),
			0,
			Input.GetAxisRaw("Vertical")
		) * moveSpeed * Time.deltaTime;
		Vector2 look = new Vector2(
			Input.GetAxis("Mouse X"),
			Input.GetAxis("Mouse Y")
		) * sensitivity;
		transform.Rotate(0, look.x, 0);
		Vector3 euler = camera.transform.localEulerAngles;
		float angle = Vector3.Angle(Vector3.up, camera.transform.forward) - 90;
		euler.x = Mathf.Clamp(angle - look.y, -90, 90);
		camera.transform.localEulerAngles = euler;
		controller.Move(transform.rotation * move);
	}

}
