using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionArea : MonoBehaviour
{
	public GameObject target;
	public string message, exitMessage;
	public bool onEnter=false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name != "Player")
			return;

		if(GetComponent<Waypoint>() != null && GetComponent<Waypoint>().canInteract == false)
			return;

		GetComponent<Animator>().SetBool("active", true);

		if(target!=null && onEnter)
			target.SendMessage(message);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.name != "Player")
			return;

		GetComponent<Animator>().SetBool("active", false);

		if(target!=null && exitMessage != "" && exitMessage != null)
			target.SendMessage(exitMessage);
	}

	void Update()
	{

		if(Input.GetKeyDown(KeyCode.F) && GetComponent<Animator>().GetBool("active"))
		{
			if(target != null)
				target.SendMessage(message);
			if(GetComponent<Waypoint>() != null)
				GetComponent<Animator>().SetBool("active", false);
		}
	}
}
