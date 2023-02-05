using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
	public static Radio Self;

	public Knob knob;
	bool isShowing = false;
	Animator animator;

	public SpriteButton trackButton;

	float startY;
	public Waypoint waypoint=null, _waypoint=null;
	string playingName = null;
	float strength
	{
		set
		{
			animator.SetFloat("strength", value);
		}

		get
		{
			return animator.GetFloat ("strength");
		}
	}

	void Awake()
	{
		startY = transform.position.y;
		Self = this;
		animator = GetComponent<Animator>();
	}

	void Start()
	{
		Hide();

		GetComponentInChildren<SpriteButton>().onClick = Track;
	}

	bool finaSpeech=false;

	void Update()
	{
		trackButton.focus = false;
		if (this.isShowing) {
			this.handleSound ();
		}
		this._waypoint = null;
		Waypoint waypoint = null;
		float min = float.MaxValue;
		foreach(Waypoint w in GameManager.Self.activeWaypoints)
		{
			float v = Mathf.Abs(knob.angle - w.frequency);
			if(v < min)
			{
				min = v;
				waypoint = w;
			}
		}

		if(waypoint == null)
		{
			_waypoint = null;
			this.waypoint = null;
			return;
		}
		strength = 0;

		if(min < 30f)
			strength = 0.5f;
		if(min < 5f)
		{
			strength = 1f;
			this._waypoint = waypoint;

			trackButton.focus = true;
		}
	}

	void handleSound() {
		if(noSound)
		{
			SoundsManager.Self.Loop(null, this.gameObject, 0.7f);
			return;
		}
		if (this.strength > 0.9f) {
			if (this.playingName != "sfx_radio-signalin-l") {
				AudioClip audio = SoundsManager.Self.GetClip ("sfx_radio-signalin");
				SoundsManager.Self.Loop (audio, this.gameObject, 0.7f);
				this.playingName = "sfx_radio-signalin-l";
			}
		} else if (this.strength > 0.49f) {
			if (this.playingName != "sfx_radio-signalin-h") {
				if (this.playingName == "sfx_radio-signalin-l") {
					this.GetComponent<AudioSource>().volume = 0.4f;
				}else{
					AudioClip audio = SoundsManager.Self.GetClip ("sfx_radio-signalin");
					SoundsManager.Self.Loop (audio, this.gameObject, 0.4f);
					this.playingName = "sfx_radio-signalin-h";
				}
			}
		} else {
			if (this.playingName != "sfx_radionoise") {
				this.playingName = "sfx_radionoise";
				AudioClip audio = SoundsManager.Self.GetClip ("sfx_radionoise");
				SoundsManager.Self.Loop (audio, this.gameObject, 0.4f);
			}
		}
	}

	bool noSound=false;

	IEnumerator FinalCoroutine()
	{
		noSound = true;
		bool note = false;
		while(true)
		{
			GameObject obj=SpeechBaloon.showBaloon(MapRadio.Self.transform.position);
			obj.GetComponent<SpeechBaloon>().setText("*To anyone on this frequency. My name is Chris. I’m travelling with my family and friends through this area.*");
			if(note==false)
				obj.GetComponent<SpeechBaloon>().playSound("sfx_dub-20b-RadioSpeech");
			yield return new WaitForSeconds(6);
			Destroy(obj);
			obj = SpeechBaloon.showBaloon(MapRadio.Self.transform.position);
			obj.GetComponent<SpeechBaloon>().setText("*We have food, medicines and a car. If you want to join us we’ll wait here fo a couple of days.*");
			yield return new WaitForSeconds(6);
			Destroy(obj);
			if(note == false)
			{
				note = true;
				Notes.Self.AddNote("I can’t believe it. I can’t send signals with this radio… I need to hurry! Please don’t leave… Don’t leave me here… I can’t stay any longer in this hellhole.", "sfx_dub-23");
				yield return new WaitForSeconds(5);
			}
		}
	}

	public void Track()
	{
		waypoint = _waypoint;

		if(!finaSpeech && waypoint!=null && waypoint.id == "note22")
		{
			finaSpeech = true;
			StartCoroutine(FinalCoroutine());
		}
	}

	public void Show()
	{
		StopCoroutine("ShowC");
		StopCoroutine("HideC");
		StartCoroutine(ShowC());
		this.isShowing = true;
	}
	public void Hide()
	{
		StopCoroutine("ShowC");
		StopCoroutine("HideC");
		StartCoroutine(HideC());
		SoundsManager.Self.Stop (this.gameObject);
		this.playingName = null;
		this.isShowing = false;
	}

	public AnimationCurve showCurve;

	IEnumerator ShowC()
	{
		float i = 0;
		float y = transform.position.y;
		while(i < 1f)
		{
			transform.position = new Vector2(transform.position.x, Mathf.Lerp(y, startY, showCurve.Evaluate(i)));

			i += Time.smoothDeltaTime*2f;
			i = Mathf.Clamp01(i);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator HideC()
	{
		float i = 0;
		float y = transform.position.y;
		while(i < 1f)
		{
			transform.position = new Vector2(transform.position.x, Mathf.Lerp(y, -1, showCurve.Evaluate(i)));

			i += Time.smoothDeltaTime;
			i = Mathf.Clamp01(i);
			yield return new WaitForEndOfFrame();
		}
	}
}
