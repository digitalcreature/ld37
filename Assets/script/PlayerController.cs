using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : SingletonBehaviour<PlayerController> {

	public float moveSpeed = 5;
	public float sensitivity = 5;

	public Vector3 lookDirection {
		get {
			return cam.transform.forward;
		}
	}

	Camera cam;
	CharacterController controller;

	void Awake() {
		controller = GetComponent<CharacterController>();
		cam = GetComponentInChildren<Camera>();
		controller.slopeLimit = 90;
		controller.stepOffset = 0;
		if (!Application.isEditor) {
			Cursor.lockState = CursorLockMode.Locked;
		}
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
		Vector3 euler = cam.transform.localEulerAngles;
		float angle = Vector3.Angle(Vector3.up, cam.transform.forward) - 90;
		euler.x = Mathf.Clamp(angle - look.y, -90, 90);
		cam.transform.localEulerAngles = euler;
		controller.Move(transform.rotation * move);
	}

}
