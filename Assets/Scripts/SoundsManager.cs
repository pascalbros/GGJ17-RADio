using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour {

	private Dictionary<string, AudioClip> dictionary = new Dictionary<string, AudioClip>();

	static public SoundsManager Self;
	// Use this for initialization
	void Start () {
		SoundsManager.Self = this;
	}

	public AudioClip GetClip(string name) {
		if (this.dictionary.ContainsKey(name)) {
			return this.dictionary [name];
		}

		AudioClip clip = (AudioClip)Resources.Load("Sounds/"+name, typeof(AudioClip));
		if (clip != null) {
			this.dictionary [name] = clip;
		}

		return clip;
	}

	public void Play(AudioClip clip, GameObject obj, float volume) {
		AudioSource source = obj.GetComponent<AudioSource> ();
		if (source == null) {
			source = obj.AddComponent<AudioSource> ();
		}
		source.volume = volume;
		source.loop = false;
		source.PlayOneShot (clip);
	}

	public void Loop(AudioClip clip, GameObject obj, float volume) {
		AudioSource source = obj.GetComponent<AudioSource> ();
		if (source == null) {
			source = obj.AddComponent<AudioSource> ();
		}
		source.clip = clip;
		source.loop = true;
		source.volume = volume;
		source.Play ();
	}

	public void Stop(GameObject obj) {
		AudioSource source = obj.GetComponent<AudioSource> ();
		if (source != null) {
			source.Stop ();
		}
	}

// How to use it
// AudioClip audio = SoundsManager.Self.GetClip ("dub/sfx_dub-01");
// SoundsManager.Self.PlayVoiceOff (audio);
	public void PlayVoiceOff(AudioClip clip) {
		GameObject voiceOff = this.GetVoiceOffObject ();

		if (voiceOff == null) {
			voiceOff = Instantiate (new GameObject ());
			voiceOff.transform.parent = Camera.main.transform;
			voiceOff.name = "VoiceOff";
			voiceOff.AddComponent<AudioSource> ();
		}

		SoundsManager.Self.Play (clip, voiceOff, 0.8f);
	}

	public void StopVoiceOff() {
		GameObject voiceOff = this.GetVoiceOffObject ();

		if (voiceOff != null) {
			voiceOff.GetComponent<AudioSource> ().Stop ();
		}
	}

	private GameObject GetVoiceOffObject() {
		Transform t = Camera.main.transform.Find ("VoiceOff");
		if (t) {
			return t.gameObject;
		}

		return null;
	}
}
