using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public Transform target;
	private float offset;


	void Start() {
		offset = this.transform.position.x - target.position.x;
	}


	void Update() {
		this.transform.position = new Vector3(target.position.x + offset, this.transform.position.y, this.transform.position.z);
	}
}
