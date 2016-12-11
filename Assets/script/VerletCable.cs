using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerletCable : MonoBehaviour {

	public Bounds roomBounds;
	public float gravity = 10;
	public int constrainIterations = 3;

	public float length = 5;
	public float thickness = 0.1f;
	public float segmentLength = 0.1f;
	public float damp = 0.1f;
	public Transform endA;
	public Transform endB;

	Vector3[] points;
	Vector3[] lastPoints;

	LineRenderer render;

	void Awake() {
		render = GetComponent<LineRenderer>();
		int count = (int) (length / segmentLength);
		points = new Vector3[count];
		lastPoints = new Vector3[count];
		for (int p = 0; p < count; p ++) {
			Vector3 point = transform.position + transform.forward * ((p * segmentLength) - (length / 2));
			points[p] = point;
			lastPoints[p] = point;
		}
	}

	void FixedUpdate() {
		float step = Time.fixedDeltaTime;
		int count = points.Length;
		Bounds roomBounds = this.roomBounds;
		roomBounds.Expand(- Vector3.one * thickness);	// account for radius when constraining to room
		// update points (gravity and velocity)
		for (int p = 0; p < count; p ++) {
			Vector3 point = points[p];
			Vector3 lastPoint = lastPoints[p];
			lastPoints[p] = point;
			Vector3 vel = point - lastPoint;
			vel += Vector3.down * gravity * step * step;
			vel *= (1 - damp);
			point += vel;
			points[p] = point;
		}
		// constrain points
		for (int i = 0; i < constrainIterations; i ++) {
			for (int p = 0; p < count; p ++) {
				Vector3 point = points[p];
				// Vector3 lastPoint = lastPoints[p];
				// distance constraints
				if (p > 0) {
					Vector3 other = points[p - 1];
					float distance = (point - other).magnitude;
					float ratio = (segmentLength - distance) / distance / 2;
					Vector3 delta = (point - other) * ratio;
					point += delta;
					other -= delta;
					points[p - 1] = other;
				}
				// constrain end points
				if (p == 0 && endA) point = endA.position;
				if (p == (count - 1) && endB) point = endB.position;
				// solve collisions with obstacles
				foreach (VerletObstacle obstacle in VerletObstacle.all) {
					Bounds bounds = obstacle.bounds;
					bounds.Expand(thickness);
					if (bounds.Contains(point)) {
						// do some math to find the nearest point on edge
						Vector3 delta = point - bounds.center;
						Vector3 sign = new Vector3(
							Mathf.Sign(delta.x),
							Mathf.Sign(delta.y),
							Mathf.Sign(delta.z)
						);
						delta = Vector3.Scale(delta, sign);	// absolute values
						delta = bounds.extents - delta;
						float min = Mathf.Min(delta.x, delta.y, delta.z);
						if (min == delta.x) point.x += sign.x * delta.x;
						else if (min == delta.y) point.y += sign.y * delta.y;
						else if (min == delta.z) point.z += sign.z * delta.z;
					}
				}
				// constraint point to room bounds
				point.x = Mathf.Clamp(point.x, roomBounds.min.x, roomBounds.max.x);
				point.y = Mathf.Clamp(point.y, roomBounds.min.y, roomBounds.max.y);
				point.z = Mathf.Clamp(point.z, roomBounds.min.z, roomBounds.max.z);
				points[p] = point;
			}
		}
		// update line renderer
		if (render && count > 0) {
			render.startWidth = thickness;
			render.endWidth = thickness;
			render.useWorldSpace = true;
			render.numPositions = count;
			render.SetPositions(points);
		}
	}

	void OnDrawGizmosSelected() {
		Color c = Gizmos.color;
		Gizmos.color = new Color(0, 0.7f, 1);
		Gizmos.DrawWireCube(roomBounds.center, roomBounds.size);
		if (points != null) {
			foreach (Vector3 point in points) {
				Gizmos.DrawWireSphere(point, thickness / 2);
			}
		}
		Gizmos.color = c;
	}

}
