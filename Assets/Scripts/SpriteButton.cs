using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteButton : MonoBehaviour
{
	public Sprite idle, click;
	SpriteRenderer renderer;

	Collider2D collider;

	public Color focusColor;
	public bool focus=false;

	public delegate void OnClick();
	public OnClick onClick;

	void Awake()
	{
		collider = GetComponent<Collider2D>();
		renderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
			Vector2 mousePosition = GuiCamera.Self.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, 1<<LayerMask.NameToLayer("GUI"));

			if(Input.GetMouseButton(0) && hitCollider == collider)
				renderer.sprite = click;
			else
				renderer.sprite = idle;

		if (Input.GetMouseButtonUp (0) && hitCollider == collider) {
			if (onClick != null) {
				onClick ();
				AudioClip audio = SoundsManager.Self.GetClip ("sfx_radio-signatrack-btn");
				SoundsManager.Self.Play (audio, this.gameObject, 0.2f);
			}
		}
		if(focus)
		{
			if((int)(Time.time*2f)%2==1)
				renderer.color = focusColor;
			else
				renderer.color = Color.white;
		}
		else
			renderer.color = Color.white;
	}
}
