using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBaloon : MonoBehaviour {

	public GameObject canvas;
	public GameObject baloonSprite;
	public SpeechBaloonSprite baloonSpriteScript;
	public UnityEngine.UI.Text textLabel;
	// Use this for initialization
	void Start () {
		this.baloonSpriteScript = this.baloonSprite.GetComponent<SpeechBaloonSprite> ();
		this.baloonSpriteScript.onBaloonOpened = delegate {
			this.onBaloonOpened ();
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onBaloonOpened() {
		Destroy (this.baloonSprite);
		this.canvas.SetActive (true);
	}

	public void setText(string text) {
		this.textLabel.text = text;
	}

	public void playSound(string name) {
		AudioClip audio = SoundsManager.Self.GetClip ("dub/"+name);
		SoundsManager.Self.PlayVoiceOff (audio);
	}

//How to use it
//GameObject obj = SpeechBaloon.showBaloon (this.transform.position);
//obj.transform.parent = this.transform;
//SpeechBaloon baloon = obj.GetComponent<SpeechBaloon> ();
//baloon.setText ("Hello world!");

	public static GameObject showBaloon(Vector3 position) {
		GameObject theObject = Resources.Load<GameObject>("SpeechBaloon");
		GameObject obj = Instantiate (theObject);
		position.z = -5;
		position.y += 0.1f;
		obj.transform.position = position;
		return obj;
	}
}
