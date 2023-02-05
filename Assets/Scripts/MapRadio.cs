using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRadio : MonoBehaviour
{
	public static MapRadio Self;

	void Awake()
	{
		Self = this;
	}
}
