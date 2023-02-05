using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
	public Transform normal, diagonal;

	void Update ()
	{
		normal.gameObject.SetActive(false);
		diagonal.gameObject.SetActive(false);

		Transform point;
		if(Radio.Self.waypoint == null)
			point = MapRadio.Self.transform;
		else
			point = Radio.Self.waypoint.transform;

		Transform marker = normal;


		Vector3 p = (point.position+Vector3.up*0.2f - Camera.main.transform.position);
		p.z = 0;
		float distance = p.magnitude;
		p.Normalize();
		p.y *= 16f / 9f;

		if(Mathf.Abs(p.x) > Mathf.Abs(p.y))
		{
			if(Mathf.Abs(Mathf.Abs(p.x) - Mathf.Abs(p.y)) < 0.2f)
				marker = diagonal;

			p.x = 1*Mathf.Sign(p.x);
			marker.transform.rotation = Quaternion.Euler(0, 0, 90);
			if(p.x < 0)
				marker.transform.localScale = new Vector3(1, 1, 1);
			else
				marker.transform.localScale = new Vector3(1, -1, 1);

		}
		if(Mathf.Abs(p.y) > Mathf.Abs(p.x))
		{
			if(Mathf.Abs(Mathf.Abs(p.x) - Mathf.Abs(p.y)) < 0.2f)
				marker = diagonal;

			p.y = 1*Mathf.Sign(p.y);
			marker.transform.rotation = Quaternion.Euler(0, 0, 0);
			if(p.y < 0)
				marker.transform.localScale = new Vector3(1, -1, 1);
			else
				marker.transform.localScale = new Vector3(1, 1, 1);
		}

		p = p.normalized * p.magnitude * 0.85f;
		p.x *= 16f / 9f;

		if(p.magnitude > distance)
		{
			marker = normal;
			marker.transform.rotation = Quaternion.Euler(0, 0, 0);
			marker.transform.localScale = new Vector3(1, -1, 1);
		}

		p = p.normalized * Mathf.Clamp(p.magnitude, 0, distance);


		p.z = -0.5f;
		marker.gameObject.SetActive(true);
		marker.transform.position = p;


	}
}
