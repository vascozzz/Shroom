using UnityEngine;
using System.Collections;

/** 
 * Player Controller
 * Responsible for moving the player character. All keyboard/mouse inputs are handled here.
 */
public class PlayerController : MonoBehaviour 
{
	[Range(0f, 10f)] public float horizontalSpeed;
	[Range(200f, 1000f)] public float flapForce;

	public AudioSource movementAudio; // two sources, as two sounds will often be playing at the same time
	public AudioSource uiAudio;

	public AudioClip deathSound;
	public AudioClip hitSound;
	public AudioClip scoreSound;
	public AudioClip jumpSound;

	private bool actionPressed = false;
	private bool dead = false;

	private Rigidbody2D rBody;
	private SpriteRenderer sRenderer;
	private Animator animController;

	private float minAngle = -58f; // min and max rotations applied to the player while flying
	private float maxAngle = -20f;
	private float minAngleVel = -2.5f; // at which velocity min and max rotations are applied
	private float maxAngleVel = -1f;

	
	void Start() {
		rBody = this.GetComponent<Rigidbody2D>();
		sRenderer = this.GetComponent<SpriteRenderer>();
		animController = this.GetComponent<Animator>();
		movementAudio = this.GetComponent<AudioSource>();
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
				animController.SetBool("Moving", true);
			}
		}

		else if (GameManager.instance.gameState == GameState.Playing) {
			HandleMovement();
		}

		else if (GameManager.instance.gameState == GameState.GameOver) {
			if (actionPressed) {
				GameManager.instance.ResetGame();
				actionPressed = false;
			}
		}
	}


	void HandleMovement() {
		// horizontal velocity, constant over time
		transform.position += new Vector3(Time.deltaTime * horizontalSpeed, 0f, 0f);
		
		// vertical velocity
		if (actionPressed) {
			rBody.velocity = Vector2.zero;
			rBody.AddForce(new Vector2(0f, flapForce));
			actionPressed = false;

			animController.SetTrigger("Jump");
			movementAudio.clip = jumpSound;
			movementAudio.Play();
		}
		
		// percentage of current velocity when compared to the limit values (eg: if current velocity == max limit, then percentage = 1.0)
		float anglePercent = (rBody.velocity.y - minAngleVel) / (maxAngleVel - minAngleVel);
		
		// actual angle based on the percentage (values outside [0,1] are automatically clamped)
		float angle = Mathf.Lerp(minAngle, maxAngle, anglePercent);
		
		this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}


	void OnCollisionEnter2D(Collision2D collision) {
		// since this is a simple game, all collisions will mean game over
		actionPressed = false;
		animController.SetTrigger("Death");

		uiAudio.clip = hitSound;
		uiAudio.Play();

		if (!dead) {
			movementAudio.clip = deathSound;
			movementAudio.Play();
			dead = true;
		}

		GameManager.instance.GameOver();
	}

	void OnTriggerEnter2D(Collider2D other) {
		// if touching objects with the "Score" tag, update score
		if (other.gameObject.tag == "Score") {
			GameManager.instance.UpdateScore();

			uiAudio.clip = scoreSound;
			uiAudio.Play();
		}
	}
}
