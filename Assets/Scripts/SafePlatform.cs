using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SafePlatformType {
	type1 = 0,
	type2 = 1,
	type3 = 2,
	type4 = 3,
	type5 = 4
}
[ExecuteInEditMode]
public class SafePlatform : MonoBehaviour {

	public Sprite[] sprites;
	public SafePlatformType type = SafePlatformType.type1;
	// Use this for initialization
	void Start () {
		this.UpdateType ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.isEditor) { this.UpdateType (); return; }
	}

	void UpdateType() {
		int index = (int)this.type;
		this.GetComponent<SpriteRenderer> ().sprite = this.sprites [index];
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			Player.Self.canBeCatched = false;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Player") {
			Player.Self.canBeCatched = true;
		}
	}
}
