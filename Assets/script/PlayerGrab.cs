using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGrab : SingletonBehaviour<PlayerGrab> {

	public LayerMask searchMask;
	public float searchRadius = 0.5f;
	public float maxGrabDistance = 5;

	public Grabbable grabbed { get; private set; }

	public LayerMask enviromentMask;
	public float grabLength = 2;

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

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (grabbed) {
				Release();
			}
			else {
				RaycastHit hit = SphereCast(searchRadius, maxGrabDistance, searchMask);
				if (hit.transform) {
					Grabbable grabbable = hit.transform.GetComponent<Grabbable>();
					if (grabbable) {
						Grab(grabbable);
					}
				}
			}
		}
	}

	void LateUpdate() {
		if (grabbed) {
			float grabLength = this.grabLength;
			RaycastHit hit = SphereCast(grabbed.radius, this.grabLength, enviromentMask);
			if (hit.transform) grabLength = hit.distance;
			grabbed.transform.position = transform.position + transform.forward * grabLength;
			Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
			if (forward != Vector3.zero)
			grabbed.transform.forward = forward;
		}
	}

	RaycastHit SphereCast(float radius, float maxDistance, LayerMask mask) {
		RaycastHit hit = new RaycastHit();
		Physics.SphereCast(transform.position, radius, transform.forward, out hit, maxDistance, mask);
		return hit;
	}

}
