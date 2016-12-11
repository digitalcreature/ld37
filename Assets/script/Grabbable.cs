using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grabbable : MonoBehaviour {

	public float radius = 0.5f;

	public bool isGrabbed { get; private set; }

	public void SetGrabbed(bool grabbed) {
		this.isGrabbed = grabbed;
		foreach (Collider collider in GetComponentsInChildren<Collider>()) {
			collider.isTrigger = grabbed;
		}
		if (grabbed) {
			OnGrab();
		}
		else {
			OnRelease();
		}
	}

	void OnDrawGizmos() {
		Color c = Gizmos.color;
		Gizmos.color = new Color(1, 0.7f, 0);
		Gizmos.DrawWireSphere(transform.position, radius);
		Gizmos.color = c;
	}

	public virtual void OnGrab() {}
	public virtual void OnRelease() {}

}
