using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Self;

	public Canvas canvas;

	public Dictionary<string,Waypoint> waypoints=new Dictionary<string, Waypoint>();
	public List<Waypoint> activeWaypoints=new List<Waypoint>();

	void Awake()
	{
		Area.areas.Clear();
		waypoints.Clear();

		Self = this;
		canvas.gameObject.SetActive(true);

		GuiCamera.Self.fader.color = Color.black;
		GuiCamera.Self.faderColor = Color.clear;
	}

	void Start()
	{
		StartCoroutine(Init());
	}

	IEnumerator Init()
	{
		yield return null;
		yield return new WaitForSeconds(1);
		ActivateWaypoint("note3");
		//ActivateWaypoint("note20");

	}

	public AnimationCurve zoomCurve;
	bool gameOver=false;
	public void GameOver()
	{
		if(gameOver)
			return;
		gameOver = true;
		GameManager.Self.canvas.gameObject.SetActive(false);
		GuiCamera.Self.faderColor = Color.black;
		StartCoroutine(GameOverC());
	}

	IEnumerator GameOverC()
	{
		yield return new WaitForSeconds(0.1f);
		float i = 0f;
		while(i <=1f)
		{
			i += Time.smoothDeltaTime/6f;
			Camera.main.orthographicSize = Mathf.Lerp(1, 0.1f, zoomCurve.Evaluate(i));
			yield return new WaitForEndOfFrame();
		}
	}

	public void ActivateWaypoint(string id)
	{
		waypoints[id].EnableInteraction();
		activeWaypoints.Add(waypoints[id]);
	}

	//called from waypoint itself
	public void DeactivateWaypoint(Waypoint waypoint)
	{
		activeWaypoints.Remove(waypoint);
	}

}
