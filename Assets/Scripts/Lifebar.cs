using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour {

	private float minimumSize = 6.0f;
	private float maximumSize = 263.0f;
	public static Lifebar Self;
	private float currentLife = 100.0f;
	public GameObject mask;
	private float timeLife = 10.0f;
	private float currentTime = 0.0f;
	// Use this for initialization
	void Start () {
		Lifebar.Self = this;
	}
	
	// Update is called once per frame
	void Update () {
		this.currentTime += Time.deltaTime;
		if (this.currentTime > this.timeLife) {
			this.currentTime = 0.0f;
			this.SetValue (this.getValue () - 1);
		}
	}

	public float getValue() {
		return this.currentLife;
	}

	public void SetValue(float value) {

		value = Mathf.Clamp (value, 0, 100);
		if (value <= 0) {
			this.currentLife = value;
			this.UpdateBar ();
			StartCoroutine(Die());
			return;
		}

		this.currentLife = value;
		this.UpdateBar ();
	}

	IEnumerator Die() {
		Player.Self.Die (false);
		yield return new WaitForSeconds(2);
		Player.Self.Die (true);
	}

	public void AddDamage(float damage)
	{
		SetValue(getValue() - damage);
	}

	private void UpdateBar() {
		float value = this.map (this.getValue (), 0.0f, 100.0f, this.minimumSize, this.maximumSize);
		RectTransform obj = this.mask.GetComponent<RectTransform> ();
		obj.sizeDelta = new Vector2(value, obj.rect.size.y);
	}

	private float map(float x, float inMin, float inMax, float outMin, float outMax) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}


}
