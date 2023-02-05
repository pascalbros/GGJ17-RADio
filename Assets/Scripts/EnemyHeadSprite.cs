using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnAttackAnimationEnd();
public delegate void OnDownAnimationEnd();
public delegate void OnIdleAnimationEnd();

public enum EnemyHeadSpriteStatus 
{
	attack = 0,
	down = 1,
	idle = 2,
	nil = 3
}
public class EnemyHeadSprite : MonoBehaviour {

	public EnemyHeadSpriteStatus status = EnemyHeadSpriteStatus.nil;
	public OnAttackAnimationEnd onAttackAnimationEnd;
	public OnDownAnimationEnd onDownAnimationEnd;
	public OnIdleAnimationEnd onIdleAnimationEnd;
	Animator animator;

	// Use this for initialization
	void Awake () {
		this.animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Enable() {
		this.gameObject.SetActive (true);
		this.status = EnemyHeadSpriteStatus.nil;
	}

	public void Disable() {
		this.gameObject.SetActive (false);
		this.status = EnemyHeadSpriteStatus.nil;
	}

	public void PlayAttackAnimation() {
		this.status = EnemyHeadSpriteStatus.attack;
		this.animator.SetInteger ("status", (int)EnemyHeadSpriteStatus.attack);
	}

	public void PlayDownAnimation() {
		this.status = EnemyHeadSpriteStatus.down;
		this.animator.SetInteger ("status", (int)EnemyHeadSpriteStatus.down);
	}

	public void PlayIdleAnimation() {
		this.status = EnemyHeadSpriteStatus.idle;
		this.animator.SetInteger ("status", (int)EnemyHeadSpriteStatus.idle);
	}

	public void onAttackEnd() {
		if (this.onAttackAnimationEnd != null) {
			this.onAttackAnimationEnd ();
		}
	}

	public void onDownEnd() {
		if (this.onDownAnimationEnd != null) {
			this.onDownAnimationEnd ();
		}
	}

	public void onIdleEnd() {
		if (this.onIdleAnimationEnd != null) {
			this.onIdleAnimationEnd ();
		}
	}
}
