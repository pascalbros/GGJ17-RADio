using UnityEngine;
using System.Collections;

[System.Serializable]
public struct EnemyStatus {
	public float radius;
	public Vector2 position;
	public Vector2 headPosition;
	[System.NonSerialized]
	public bool isEnabled;
	public float speed;

	public EnemyStatus(float radius, Vector3 position, float speed) {
		this.radius = radius;
		this.position = position;
		this.speed = speed;
		this.isEnabled = true;
		this.headPosition = position;
	}
}

public class Enemy : MonoBehaviour {

	private int Z = 0;
	public GameObject head;
	[System.NonSerialized]
	public EnemyHead headController;

	public EnemyStatus status = new EnemyStatus ();
	// Use this for initialization
	void Start () {
		this.Setup ();
	}

	private void Setup() {
		this.status.isEnabled = true;
		this.status.position = new Vector3(this.transform.position.x, this.transform.position.y, Z);
		this.headController = this.head.GetComponent<EnemyHead> ();
		this.headController.status.position = this.status.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (!this.isEnabled()) {
			return;
		}

		if (this.headController.enemyHeadSpriteScript.status == EnemyHeadSpriteStatus.nil) {
			Vector3 position = Player.Self.transform.position;
			this.headController.isFollowingThePlayer = true;
			if (!Player.Self.canBeCatched) {
				//Forces to go back to the center
				position = new Vector3 (99999, 99999, 0);
				this.headController.isFollowingThePlayer = false;
			}
			GameObject rock = RocksManager.Instance.getRock ((Vector2)this.status.position, this.status.radius);
			if (rock != null) {
				position = rock.transform.position;
				this.headController.isFollowingThePlayer = false;
			}

			position.y = position.y - 0.08f;

			this.headController.MoveTo (position, this.status.speed, this.status.radius);
		}
	}

	public void setEnabled(bool enabled) {
		this.status.isEnabled = enabled;
	}

	public bool isEnabled() {
		return this.status.isEnabled;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 00, 0, 0.5f);
		Gizmos.DrawWireSphere(transform.position, status.radius);
	}
}
