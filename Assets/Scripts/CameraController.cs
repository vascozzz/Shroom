using UnityEngine;
using System.Collections;

/** 
 * Camera Controller
 * Responsible for moving the camera on a target (x-axis only).
 * Used to follow the player.
 */
public class CameraController : MonoBehaviour 
{
	public Transform target;

	[Range(0f, 2f)] public float shakeDuration = 1f;
	[Range(0f, 1f)] public float shakeAmount = 0.7f;

	private int backgroundLayer = 10; // layer applied to all background objects
	private Vector3 shakePos = Vector3.zero;
	private float offset; // distance relative to target at all times


	void Start() {
		offset = this.transform.position.x - target.position.x;
	}


	void Update() {
		// target locking
		if (GameManager.instance.gameState == GameState.Playing) {
			this.transform.position = new Vector3(target.position.x + offset, this.transform.position.y, this.transform.position.z);
		}

		// camera shake on player impact
		else if (GameManager.instance.gameState == GameState.GameOver) {
			// before shaking, save current position
			if (shakePos == Vector3.zero) {
				shakePos = this.transform.position;

				// and move all children that are background objects (otherwise, the "shaking" effect won't be visible)
				foreach (Transform child in this.transform) {
					if (child.gameObject.layer == backgroundLayer) {
						child.parent = null;
					}
				}
			}

			// finally, use a random vector to generate the shaking effect
			if (shakeDuration > 0) {
				Vector3 shake = new Vector3((Random.insideUnitSphere * shakeAmount).x, 0f, 0f);
				this.transform.position = shakePos + shake;

				shakeDuration -= Time.deltaTime;
			}
		}
	}
}
