using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPuddle : MonoBehaviour {

	private float elapsedTime = 0.0f;
	private float time = 10.0f;
	private float fadeTime = 3.0f;
	private bool canFade = false;
	// Use this for initialization
	void Start () {
		Vector3 pos = this.transform.position;
		pos.z = -1;
		this.transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {

		this.elapsedTime += Time.fixedDeltaTime;
		if (this.canFade) {
			float alpha = 1.0f - this.map (this.elapsedTime, 0.0f, this.fadeTime, 0.0f, 1.0f);
			Color c = this.GetComponent<SpriteRenderer> ().color;
			c.a = alpha;
			this.GetComponent<SpriteRenderer> ().color = c;

			if (this.elapsedTime >= this.fadeTime || alpha <= 0.0f) {
				Destroy (this.gameObject);
			}
		} else {
			if (this.elapsedTime >= this.time) {
				this.elapsedTime = 0.0f;
				this.canFade = true;
			}
		}
	}

	private float map(float x, float inMin, float inMax, float outMin, float outMax) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
