using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RocksManager
{
	public List<GameObject> rocks = new List<GameObject>();
	private RocksManager() {
	}

	public static RocksManager Instance { get { return Nested.instance; } }

	public GameObject getRock(Vector2 position, float radius) {
		for (int i = 0; i < this.rocks.Count; i++) {
			GameObject rock = this.rocks [i];
			float distance = Vector2.Distance (position, (Vector2)rock.transform.position);
			if (distance < radius) {
				return rock;
			}
		}

		return null;
	}

	private class Nested {
		static Nested() { }

		internal static readonly RocksManager instance = new RocksManager();
	}
}