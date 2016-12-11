using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerletCableHandle : Grabbable {

	public bool isPluggedIn { get; private set; }

	public bool isStatic {
		get {
			return isPluggedIn || isGrabbed;
		}
	}


}
