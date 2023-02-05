using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaypointHandler: MonoBehaviour {
	public abstract void onWaypointTrigger (Waypoint script);
}

public class Waypoint : MonoBehaviour {

	public string id
	{
		get
		{
			return name;
		}
	}
	public float frequency;
	[Multiline]
	public string note, voice;
	public string[] nextWaypoints;
	public string[] unlockAreas;
	public bool destroyOnInteract=false;

	public WaypointHandler handler;

	private const KeyCode interactionKey = KeyCode.F;
	public bool canInteract = false;
	private bool isPlayerInside = false;
	// Use this for initialization
	void Start () {
		try
		{
			GameManager.Self.waypoints.Add(id, this);
		}
		catch
		{
			Debug.LogError(id);
		}

		GetComponent<Renderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		transform.GetChild(0).GetComponent<Collider2D>().enabled = false;

		frequency = Random.Range(-175f, 175f);
		}
	
	// Update is called once per frame
	void Update () {
		if(this.canInteract && this.isPlayerInside) 
		{
			this.DoInteraction();
		}
	}

	public void EnableInteraction()
	{
		GetComponent<Renderer>().enabled = true;
		transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
		canInteract = true;
	}

	private void DoInteraction() {
		GetComponent<Collider2D>().enabled = false;
		//transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
		Radio.Self.waypoint = null;
		this.canInteract = false;
		GameManager.Self.DeactivateWaypoint(this);
		if (this.handler != null) {
			this.handler.onWaypointTrigger (this);
		} else {
			//Debug.LogError ("No handler found in Waypoint script");
		}

		if(note!="")
			Notes.Self.AddNote(note, voice);

		foreach(string w in nextWaypoints)
			GameManager.Self.ActivateWaypoint(w);

		foreach(string area in unlockAreas)
			Area.UnlockArea(area);

		if(destroyOnInteract)
			gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.name=="Player")
		this.isPlayerInside = true;
	}
	void OnTriggerExit2D(Collider2D other) {
		if(other.name=="Player")
		this.isPlayerInside = false;
	}
}
