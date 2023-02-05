using UnityEngine;
using System.Collections;

public enum PlayerStatus {
	idle = 0,
	walking = 1,
	dead = 2
}

public class Player : MonoBehaviour
{
	public static Player Self;
	Rigidbody2D rigidbody;

	public float speed=1;
	public float deceleration=20;
	public float acelleration=25;
	public PlayerStatus status = PlayerStatus.idle;
	private GameObject soundWave;
	Vector2 direction;

	Rigidbody2D pickedObj=null;

	Animator animator;
	public Transform shadow;
	Vector3 force;

	public GameObject blood;

	private int enteredCounter = 0;
	public bool canBeCatched {

		get {
			return enteredCounter > 0 ? false : true;
		}
		set {
			if (!value) {
				enteredCounter += 1;
			} else {
				enteredCounter -= 1;
			}
			enteredCounter = Mathf.Clamp (enteredCounter, 0, 9999);
		}
	}

	private const float walkingTime = 0.3f;
	private float currentWalkingTime = 0.0f;

	void Awake()
	{
		Self = this;
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		this.soundWave = this.transform.Find ("soundwave").gameObject;
	}

	void Update()
	{
		animator.SetBool("carry", pickedObj != null);

		if (this.status == PlayerStatus.dead) {
			animator.SetFloat("speed", 0);
			canBeCatched = false;
			rigidbody.velocity = Vector2.zero;
			return;
		}
		if(Time.timeScale == 0f)
		{
			rigidbody.velocity = Vector3.zero;
			return;
		}

			Vector2 mouseDirection= Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if(Mathf.Abs(x) > 0.5f)
			x = 1f * Mathf.Sign(x);
		else
			x = 0;
		if(Mathf.Abs(y) > 0.5f)
			y = 1f * Mathf.Sign(y);
		else
			y = 0;

		Vector2 input = new Vector2(x, y);
		input.Normalize();

		Vector2 v = input*speed;
		if(v.magnitude < rigidbody.velocity.magnitude)
		{
			v = Vector3.Lerp(rigidbody.velocity, v, Time.smoothDeltaTime * deceleration);
			if(v.magnitude < 0.1f)
				v = Vector2.zero;
		}
		else
		{
			v=Vector3.Lerp(rigidbody.velocity, v, Time.smoothDeltaTime * acelleration);
		}

		if(input.magnitude > 0.1f)
		{
			direction = input;
			animator.SetFloat("x", direction.x);
			animator.SetFloat("y", direction.y);
			if(direction.x > 0)
				transform.localScale = Vector3.one;
			else
				transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			direction = mouseDirection;
			animator.SetFloat("x", direction.x);
			animator.SetFloat("y", direction.y);
			if(direction.x > 0)
				transform.localScale = Vector3.one;
			else
				transform.localScale = new Vector3(-1, 1, 1);
		}

	
		animator.SetFloat("speed", v.magnitude);
		if (animator.GetFloat ("speed") > 0) {
			this.status = PlayerStatus.walking;
		} else {
			this.status = PlayerStatus.idle;
		}
		

		if(pickedObj != null)
		{
			//rigidbody.velocity = Vector3.zero;
			//animator.SetFloat("speed", 0);
			pickedObj.transform.localPosition = Vector3.Lerp(pickedObj.transform.localPosition, Vector3.up * 0.1f, Time.smoothDeltaTime * 10f);
			pickedObj.transform.Translate(0, 0, -0.5f);

			direction = mouseDirection;
			//animator.SetFloat("x", direction.x);
			//animator.SetFloat("y", direction.y);
			//if(direction.x > 0)
			//	transform.localScale = Vector3.one;
			//else
			//	transform.localScale = new Vector3(-1, 1, 1);

			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
			{
				Throw();	
			}
		}


		rigidbody.velocity = v;
		rigidbody.velocity += (Vector2)force;
		if(force.magnitude > 0f)
		{
			float t = 0.75f;
			if(Time.time - hitTime < t)
			{
				float dropI = (Time.time - hitTime) / t;
				float yOffset = 0;
				float yOffsetShadow = 0;

				if(dropI > 0.3f)
				{
					yOffset = ((Mathf.Abs(-Mathf.Sin((dropI - 0.3f) *5.5f))) / 1.5f - 0.22f) * 130f * (1f - (dropI - 0.3f));
					yOffsetShadow = ((Mathf.Abs(-Mathf.Cos((dropI - 0.3f) * 5.5f))) / 1.5f - 0.22f) * 130f * (1f - (dropI - 0.3f));
				}

				rigidbody.velocity = ((Vector2)force.normalized * 150f * Time.deltaTime * (1f - dropI)) + Vector2.up * yOffset * Time.deltaTime;
				//shadow.transform.localPosition = new Vector3(0,  yOffsetShadow * 0.006f, 1f);
			}
		}

		if(Input.GetKeyDown(KeyCode.F))
		{
			if(pickedObj == null)
			{
				RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.25f, 1 << LayerMask.NameToLayer("Throwable"));

				if(hit.collider != null)
				{
					Pick(hit.collider);
				}
			}
		}

		force = Vector3.MoveTowards(force, Vector3.zero, Time.smoothDeltaTime * 2f);

		this.UpdateWalkingSteps ();
		this.HandleSoundWave ();
	}

	void HandleSoundWave() {

		if (!this.canBeCatched) {
			this.soundWave.SetActive (false);
			return;
		}

		switch (this.status) {
		case PlayerStatus.walking:
			if (!this.soundWave.activeSelf) {
				this.soundWave.SetActive (true);
			}
			break;
		case PlayerStatus.idle:
			if (this.soundWave.activeSelf) {
				this.soundWave.SetActive (false);
			}
			break;
		case PlayerStatus.dead:
			if (this.soundWave.activeSelf) {
				this.soundWave.SetActive (false);
			}
			break;
		default:
			break;
		}
	}

	public void Throw()
	{
		pickedObj.transform.parent = null;
		FlyingRock f = pickedObj.gameObject.AddComponent<FlyingRock>();
		f.direction = direction;
		pickedObj = null;
	}

	void UpdateWalkingSteps() {
		if (this.status == PlayerStatus.walking && this.canBeCatched) {
			this.currentWalkingTime += Time.fixedDeltaTime;
			if (this.currentWalkingTime > Player.walkingTime) {
				AudioClip audio = SoundsManager.Self.GetClip ("sfx_char_steps");
				SoundsManager.Self.Play (audio, this.gameObject, 0.2f);
				this.currentWalkingTime = 0.0f;
			}
		} else {
			this.currentWalkingTime = 0.0f;
		}
	}

	void Pick(Collider2D obj)
	{
		obj.enabled = false;
		pickedObj = obj.GetComponent<Rigidbody2D>();
		pickedObj.isKinematic = true;
		pickedObj.transform.parent = transform;

		AudioClip audio = SoundsManager.Self.GetClip ("sfx_char-pickup");
		SoundsManager.Self.Play (audio, pickedObj.gameObject, 0.2f);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.layer != LayerMask.NameToLayer("Throwable"))
			return;

		if(pickedObj != null)
			return;

		Pick(coll.collider);
	}

	void LateUpdate()
	{
	}

	float hitTime;
	public void Hit(Vector3 pos)
	{
		hitTime = Time.time;
		force=((Vector2)transform.position- (Vector2)pos).normalized*0.2f;

		Lifebar.Self.AddDamage(20);

		GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("blood"));
		go.transform.position = transform.position+Vector3.up*0.1f;
		Destroy(go, 0.5f);
		this.Bleed ();
	}

	public void Die(bool animation) {
		if(pickedObj != null)
			Throw();

		if (!animation) {
			this.canBeCatched = false;
			this.status = PlayerStatus.dead;
		} else {
			AudioClip audio = SoundsManager.Self.GetClip ("sfx_char-death");
			SoundsManager.Self.Play (audio, this.gameObject, 0.8f);
			animator.SetBool ("dead", true);
			this.canBeCatched = false;
			this.status = PlayerStatus.dead;
		}
		
	}

	public void Bleed() {
		Vector3 pos = this.transform.position;
		pos.y -= 0.1f;
		pos.z = -1;

		GameObject blood = Instantiate (this.blood);
		blood.transform.position = pos;
	}

	public void onPlayerDead() {
		GameManager.Self.GameOver ();
		StartCoroutine(Restart());
	}

	IEnumerator Restart()
	{
		yield return new WaitForSeconds(9);
		Application.LoadLevel("intro");
	}
}
