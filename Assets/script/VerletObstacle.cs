using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// a boundingbox that excludes verlet based objects
public class VerletObstacle : MonoBehaviour {

	public Bounds localBounds;

	public Bounds bounds {
		get {
			Bounds bounds = localBounds;
			bounds.center += transform.position;
			return bounds;
		}
	}

	static HashSet<VerletObstacle> _all;
	public static HashSet<VerletObstacle> all {
		get {
			if (_all == null) {
				_all = new HashSet<VerletObstacle>();
			}
			return _all;
		}
	}

	void OnEnable() {
		all.Add(this);
	}

	void OnDisable() {
		all.Remove(this);
	}

	void OnDrawGizmosSelected() {
		Color c = Gizmos.color;
		Gizmos.color = new Color(0, 0.7f, 1);
		Gizmos.DrawWireCube(bounds.center, bounds.size);
		Gizmos.color = c;
	}

}
