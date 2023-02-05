using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyKit : MonoBehaviour {

	private float plus = 40;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			AudioClip audio = SoundsManager.Self.GetClip ("sfx_char-pickuphealth");
			SoundsManager.Self.Play (audio, Camera.main.gameObject, 0.8f);

			Lifebar.Self.SetValue (Lifebar.Self.getValue() + plus);
			Destroy (this.gameObject);
		}
	}
}
