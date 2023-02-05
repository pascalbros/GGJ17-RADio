using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupObject : MonoBehaviour
{
	[Multiline]
	public string text;

	GameObject baloon;
	public void Show()
	{
		return;
		baloon = SpeechBaloon.showBaloon(transform.position);
		baloon.GetComponent<SpeechBaloon>().setText(text);
		Destroy(baloon, 4);
	}
	public void Hide()
	{
		return;
		if(baloon!=null)
			Destroy(baloon);
	}
}
