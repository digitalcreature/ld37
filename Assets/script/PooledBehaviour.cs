using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PooledBehaviour<T, D> : MonoBehaviour
	where T : PooledBehaviour<T, D>
	where D : class {

	private static HashSet<T> _pool;
	protected static HashSet<T> pool {
		get {
			if (_pool == null) _pool = new HashSet<T>();
			return _pool;
		}
	}

	public static T Allocate(D data = null) {
		T t = null;
		if (pool.Count == 0) {
			t = new GameObject("pooled " + typeof(T).Name + " instance").AddComponent<T>();
			t.OnCreate();
		}
		else {
			foreach (T tt in pool) {
				t = tt;
				break;
			}
			pool.Remove(t);
		}
		t.gameObject.SetActive(true);
		t.OnAllocate(data);
		return t;
	}

	public static void Free(T t) {
		if (t != null) {
			t.OnFree();
			t.gameObject.SetActive(false);
			pool.Add(t);
		}
	}

	public virtual void OnCreate() {}
	public virtual void OnAllocate(D data) {}
	public virtual void OnFree() {}

}
