using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiCamera : MonoBehaviour
{
	public static GuiCamera Self;

	public SpriteRenderer fader;

	public Color faderColor;
	public float fadeSpeed=1;

	void Awake()
	{
		Self = this;
	}

	void Update()
	{
		fader.color = Color.Lerp(fader.color, faderColor, Time.smoothDeltaTime * 0.5f*fadeSpeed);
	}
}
