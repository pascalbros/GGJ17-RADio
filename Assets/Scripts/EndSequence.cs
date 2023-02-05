using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequence : MonoBehaviour
{
	public Enemy[] enemies;

	public GameObject endTitle;

	public void Begin()
	{
		Player.Self.speed = 0;

		StartCoroutine(GameOver());
	}

	IEnumerator GameOver()
	{
		GameObject obj = SpeechBaloon.showBaloon(EndRadio.Self.transform.position);
		obj.GetComponent<SpeechBaloon>().setText("*To anyone on this frequency. My name is Chris. I’m travelling with my family and friends through this area.*");
		obj.GetComponent<SpeechBaloon>().playSound("sfx_dub-20b-RadioSpeech");
		yield return new WaitForSeconds(6f);
		Destroy(obj);
		obj = SpeechBaloon.showBaloon(EndRadio.Self.transform.position);
		obj.GetComponent<SpeechBaloon>().setText("*We have food, medicines and a car. If you want to join us we’ll wait here fo a couple of days.*");
		yield return new WaitForSeconds(5f);
		Destroy(obj);
		yield return new WaitForSeconds(2.5f);
		obj = SpeechBaloon.showBaloon(Player.Self.transform.position);
		obj.GetComponent<SpeechBaloon>().setText("I can't fucking belive it.");
		obj.GetComponent<SpeechBaloon>().playSound("sfx_dub-24");

		StartCoroutine(Sound());
		yield return new WaitForSeconds(2.5f);
		Destroy(obj);
		yield return new WaitForSeconds(0.5f);


		GameManager.Self.canvas.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.8f);
		foreach(Enemy e in enemies)
			e.gameObject.SetActive(true);
		yield return new WaitForSeconds(2.3f);
		GuiCamera.Self.fadeSpeed = 100f;
		Player.Self.canBeCatched = false;
		GameManager.Self.GameOver();
		yield return new WaitForSeconds(0.5f);
		endTitle.SetActive(true);
		yield return new WaitForSeconds(10.0f);

		Application.LoadLevel("intro");
	}

	IEnumerator Sound()
	{
		yield return new WaitForSeconds(2.5f);
		SoundsManager.Self.Play(SoundsManager.Self.GetClip("sfx_finalcrescendo"), gameObject, 1);
	}
	
}
