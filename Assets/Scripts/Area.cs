using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
	public static Dictionary<string, Area> areas=new Dictionary<string, Area>();

	public SpriteRenderer radioUpgrade;

	public float size
	{
		get
		{
			return GetComponent<CircleCollider2D>().radius;
		}
	}

	void Start ()
	{
		areas.Add(name, this);	
	}

	void onDestroy()
	{
		areas.Remove(name);
	}

	public static void UnlockArea(string area)
	{
		if(areas[area].radioUpgrade != null)
			areas[area].radioUpgrade.enabled = true;
		areas.Remove(area);
		AudioClip audio = SoundsManager.Self.GetClip ("sfx_radioupgrade-added");
		SoundsManager.Self.Play (audio, Camera.main.gameObject, 0.4f);
	}

}
