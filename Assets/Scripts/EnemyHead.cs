using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyHeadStatus {
	public Vector2 position;
}

public class EnemyHead : MonoBehaviour {

	public EnemyHeadStatus status = new EnemyHeadStatus ();
	public GameObject bumpObject;
	public GameObject enemyHeadSprite;
	public EnemyHeadSprite enemyHeadSpriteScript;
	public bool isPlayerInside = false;
	private Vector2 lastPosition;
	private float minimumDistanceBeforeAnimation = 0.07f;
	public bool isFollowingThePlayer = false;
	private bool canBumpGround = true;
	// Use this for initialization
	void Start () {
		this.lastPosition = this.transform.position;
		EnemyHeadSprite headSpriteController = this.enemyHeadSprite.GetComponent<EnemyHeadSprite> ();
		headSpriteController.onAttackAnimationEnd = delegate() {
			this.onAttackAnimationEnd ();
		};
		headSpriteController.onDownAnimationEnd = delegate() {
			this.onDownAnimationEnd();
		};
		headSpriteController.onIdleAnimationEnd = delegate() {
			this.onIdleAnimationEnd();
		};
		this.hideEnemySprite ();
		//this.enemyHeadSpriteScript = this.enemyHeadSprite.GetComponent<EnemyHeadSprite> ();
	}

	void Update () {
		this.transform.position = this.status.position;
		this.CheckLastPosition ();
	}

	void CheckLastPosition() {
		float distance = Vector2.Distance ((Vector2)this.lastPosition, (Vector2)this.transform.position);
		if (distance > this.minimumDistanceBeforeAnimation) {
			this.AnimateGround ();
			this.lastPosition = this.transform.position;
		}
	}

	void AnimateGround() {
		if (!this.canBumpGround) {
			return;
		}
		GameObject bump = Instantiate(this.bumpObject);
		bump.transform.position = this.lastPosition;
		bump.transform.SetParent (this.transform.parent);
	}

	public void MoveTo(Vector2 position, float speed, float maxDistance) {

		float distance = Vector2.Distance ((Vector2)this.transform.parent.position, position);
		if (distance > maxDistance) {
			position = this.transform.parent.position;
			this.isFollowingThePlayer = false;
		}
		float step = speed * Time.smoothDeltaTime;
		Vector2 direction = position - (Vector2)this.transform.position;
		step = Mathf.Clamp (step, 0, direction.magnitude);
		direction.Normalize ();
		this.status.position += direction * step;
	}

	void hideEnemySprite() {
		this.enemyHeadSpriteScript.Disable ();
	}

	void showEnemySprite() {

		Vector3 scale = this.enemyHeadSprite.transform.localScale;
		if (this.transform.position.x < this.lastPosition.x) {
			scale.x = -1;
		} else {
			scale.x = 1;
		}
		this.enemyHeadSprite.transform.localScale = scale;
		this.enemyHeadSpriteScript.Enable ();
		this.enemyHeadSpriteScript.PlayAttackAnimation ();
		AudioClip audio = SoundsManager.Self.GetClip ("sfx_enm-jumpingout");
		SoundsManager.Self.Play (audio, this.gameObject, 0.2f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			this.isPlayerInside = true;
			if (Player.Self.canBeCatched && this.isFollowingThePlayer) {
				this.showEnemySprite ();
				StartCoroutine (HitPlayer ());
			}
		} else if (other.name.Contains ("Platform")) {
			this.canBumpGround = false;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Player") {
			this.isPlayerInside = false;
		} else if (other.name.Contains ("Platform")) {
			this.canBumpGround = true;
		}
	}

	IEnumerator HitPlayer()
	{
		yield return new WaitForSeconds(0.25f);
		Player.Self.Hit(transform.position);
	}

	void onDownAnimationEnd() {
		this.hideEnemySprite ();
	}

	void onAttackAnimationEnd() {
		this.enemyHeadSpriteScript.PlayIdleAnimation ();
		if (this.isPlayerInside) {
			Debug.Log ("Hit!");
		}
	}

	void onIdleAnimationEnd() {
		this.enemyHeadSpriteScript.PlayDownAnimation ();
	}
}
