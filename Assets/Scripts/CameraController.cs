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
	private float offset; // distance relative to target at all times


	void Start() {
		offset = this.transform.position.x - target.position.x;
	}


	void Update() {
		this.transform.position = new Vector3(target.position.x + offset, this.transform.position.y, this.transform.position.z);
	}
}
