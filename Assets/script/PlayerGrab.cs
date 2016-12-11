using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGrab : SingletonBehaviour<PlayerGrab> {

	public Grabbable grabbed { get; private set; }

	public void Grab(Grabbable grabbable) {
		Release();
		grabbable.SetGrabbed(true);
		grabbed = grabbable;
	}

	public void Release() {
		if (grabbed) {
			grabbed.SetGrabbed(false);
			grabbed = null;
		}
	}

	public void Update() {
		if (grabbed) {
			Vector3 look = transform.forward;
			grabbed.transform.position = transform.position + look * 2;
			grabbed.transform.forward = Vector3.ProjectOnPlane(look, Vector3.up);
		}
	}

}
