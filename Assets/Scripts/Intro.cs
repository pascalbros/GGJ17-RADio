using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
	public Sprite[] sprites;

	void Start()
	{
		Screen.SetResolution(1280, 720, false);
	}

	float frameTime = 0.08f;
	void Update()
	{
		if(Input.anyKeyDown)
			Application.LoadLevel("main");
	}
}
