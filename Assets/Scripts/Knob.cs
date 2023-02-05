using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour
{
	bool rotation=false;

	public Transform marker;

	public float angle;

	void Update ()
	{
		Vector2 mousePosition = GuiCamera.Self.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

		if(hitCollider == GetComponent<Collider2D>())
		{
			if(Input.GetMouseButtonDown(0))
				rotation = true;
		}


		if(Input.GetMouseButton(0) == false)
			rotation = false;

		Vector3 dir = (Vector2)transform.position - mousePosition;
		dir.Normalize();
		angle = Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;
		if(rotation)
		{
			Vector3 p = -dir * 0.0465f;
			p.z = -0.5f;
			transform.GetChild(0).localPosition = p;
		}

		dir = transform.GetChild(0).localPosition;
		dir.Normalize();
		angle = Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;
		Vector3 pp = Vector3.Lerp(Vector3.zero, Vector3.right * 0.5f, (angle+180f) / 359f);
		pp.z = -0.5f;
		marker.transform.localPosition = pp;
	}
}
