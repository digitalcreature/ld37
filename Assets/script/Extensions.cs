using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions {

	public static T AddComponent<T>(this MonoBehaviour mb) where T : Component {
		return mb.gameObject.AddComponent<T>();
	}

}
