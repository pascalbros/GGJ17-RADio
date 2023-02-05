using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRock : MonoBehaviour
{
	public Vector2 direction;

	public float startTime;

	bool smashed=false;

	void Start ()
	{
		startTime = Time.time;
	}
	
	void Update ()
	{
		float t = Mathf.Clamp01((Time.time - startTime)/0.35f);
		transform.position += (Vector3)direction * Time.smoothDeltaTime * 5f*(1f-t);

		if(t > 0.4f && smashed==false)
		{
			GetComponent<Animator>().SetTrigger("smash");
			smashed = true;
			RocksManager.Instance.rocks.Add(gameObject);
			Destroy(gameObject, 4);

			StartCoroutine(Effect());
		}
		if(t<0.75f)
		{
			transform.position += Vector3.down * Time.smoothDeltaTime * 0.5f;
		}
	}

	IEnumerator Effect()
	{
		AudioClip audio = SoundsManager.Self.GetClip ("sfx_stone-hit");
		SoundsManager.Self.Play (audio, this.gameObject, 0.5f);
		yield return new WaitForSeconds(0.2f);
		GameObject sfx = Instantiate<GameObject>(Resources.Load<GameObject>("soundwave"));
		Vector3 p = transform.position;
		p.z = 2;
		sfx.transform.position = p;
			Destroy(sfx, 0.667f);
	}

	void OnDestroy()
	{
		RocksManager.Instance.rocks.Remove(gameObject);
	}
}
