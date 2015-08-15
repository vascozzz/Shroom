using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	[Range(0f, 10f)] public float horizontalSpeed;
	[Range(200f, 1000f)] public float flapForce;

	private Rigidbody2D rBody;
	private SpriteRenderer sRenderer;
	private bool actionPressed = false;

	private float minAngle = -58f;
	private float maxAngle = -20f;
	private float minAngleVel = -2.5f;
	private float maxAngleVel = -1f;

	
	void Start() {
		rBody = this.GetComponent<Rigidbody2D>();
		sRenderer = this.GetComponent<SpriteRenderer>();
	}


	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
			actionPressed = true;
		}
	}


	void FixedUpdate() {
		if (GameManager.instance.gameState == GameState.Intro) {
			if (actionPressed) {
				GameManager.instance.GameStart();
				sRenderer.enabled = true;
				rBody.isKinematic = false;
			}
		}

		if (GameManager.instance.gameState == GameState.Playing) {
			HandleMovement();
		}
	}


	void HandleMovement() {
		// horizontal velocity
		transform.position += new Vector3(Time.deltaTime * horizontalSpeed, 0f, 0f);
		
		// vertical velocity
		if (actionPressed) {
			rBody.velocity = Vector2.zero;
			rBody.AddForce(new Vector2(0f, flapForce));
			actionPressed = false;
		}
		
		// percentage of current velocity when compared to the limit values (eg: if current velocity = max limit, then percentage = 1.0)
		float anglePercent = (rBody.velocity.y - minAngleVel) / (maxAngleVel - minAngleVel);
		
		// actual angle based on the percentage (values outside [0,1] are automatically clamped)
		float angle = Mathf.Lerp(minAngle, maxAngle, anglePercent);
		
		this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}


	void OnCollisionEnter2D(Collision2D collision) {
		actionPressed = false;
		GameManager.instance.GameOver();
	}

	void OnTriggerEnter2D(Collider2D other) {
		// if touching objects with the "Score" tag, update score
		if (other.gameObject.tag == "Score") {
			GameManager.instance.UpdateScore();
		}
	}
}
