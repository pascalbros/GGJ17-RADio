using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBumpController : MonoBehaviour {
	static float lastPlayTime = 0;
	// Use this for initialization

	void Awake() {
		SandBumpController.lastPlayTime = 0.0f;
	}

	void Start () {
		Vector3 pos = this.transform.position;
		pos.z = 95;
		this.transform.position = pos;

		if (Time.timeSinceLevelLoad - SandBumpController.lastPlayTime > 0.15f) {
			AudioClip audio = SoundsManager.Self.GetClip ("sfx_sandbump");
			SoundsManager.Self.Play (audio, this.gameObject, 0.03f);
			SandBumpController.lastPlayTime = Time.timeSinceLevelLoad;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void removeFromScene() {
		Destroy (this.gameObject);
	}
}
