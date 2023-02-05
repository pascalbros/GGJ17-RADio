using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
	public AudioClip music;

	void Start ()
	{
		GetComponent<AudioSource>().clip = music;
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
	}
	
}
