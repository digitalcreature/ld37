using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grabbable : MonoBehaviour {

	Rigidbody body;

	public bool isGrabbed { get; private set; }

	void Awake() {
		body = GetComponent<Rigidbody>();
	}

	public void SetGrabbed(bool grabbed) {
		this.isGrabbed = grabbed;
		body.isKinematic = grabbed;
	}

}
