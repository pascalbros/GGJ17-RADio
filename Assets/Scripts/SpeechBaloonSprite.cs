using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnBaloonOpened();

public class SpeechBaloonSprite : MonoBehaviour {

	public OnBaloonOpened onBaloonOpened;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onOpened() {
		if (this.onBaloonOpened != null) {
			this.onBaloonOpened ();
		}
	}
}
