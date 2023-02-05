using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
	public static GameCamera Self;

	public float speed;

	void Awake()
	{
		Self = this;
	}

	Vector3 vel=Vector3.zero;
	void Update()
	{
		Vector3 destination = Player.Self.transform.position;
		destination.z = -10;
		Vector3 p = Vector3.SmoothDamp(transform.position, destination, ref vel, speed, 100000, Time.smoothDeltaTime);

		foreach(string area in Area.areas.Keys)
		{
			if(Vector3.Distance(Area.areas[area].transform.position, p) < Area.areas[area].size)
				return;
		}

		p.x = Mathf.Clamp(p.x, -9.589453f, 9.589453f);
		p.y = Mathf.Clamp(p.y, -10.3f, 10.3f);

		transform.position = p;
	}
}
