using UnityEngine;
using System.Collections;

/**
 * Moving Obstacle Controller
 * Responsible for moving an obstacle up and down, between two points.
 */
public class MovingObstacleController : MonoBehaviour 
{
	public float minSpeed = 1f;
	public float maxSpeed = 1.5f;

	public float minHeight = -5.3f;
	public float maxHeight = 5.3f;

	private float speed;
	private Vector3 goal;
	private Vector3 nextGoal;



	void Start() {
		speed = Random.Range(minSpeed, maxSpeed);
		goal = new Vector3(this.transform.localPosition.x, minHeight, this.transform.localPosition.z);
		nextGoal = new Vector3(this.transform.localPosition.x, maxHeight, this.transform.localPosition.z);
		
	}


	void Update() {
		if (GameManager.instance.gameState == GameState.Playing) {
			this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, goal, Time.deltaTime * speed);
			
			if (this.transform.localPosition == goal) {
				Vector3 temp = goal;
				goal = nextGoal;
				nextGoal = temp;
			}
		}
	}
}