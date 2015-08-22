using UnityEngine;
using System.Collections;

/**
 * Spawner Controller
 * Responsible for spawning random obstacles for the player.
 */
public class ObstaclesController : MonoBehaviour 
{
	public GameObject[] obstacles;
	public GameObject container;

	public float minSpawnInterval = 1f; // expressed in seconds
	public float maxSpawnInterval = 2f;
	public float obstaclesHeightVariance = 0.4f;

	public float pointsToSecondStage = 10;


	void Start() {
		// get the camera's horizontal boundary as a world point
		float screenHorizontalBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane)).x + 1f;

		// set the spawner's position so that it is never visible to the player
		float horizontalPos = Mathf.Max(this.transform.localPosition.x, screenHorizontalBoundary);
		this.transform.localPosition = new Vector3(horizontalPos, this.transform.localPosition.y, this.transform.localPosition.z);

		// invoke the spawn function, which will then invoke itself continuously overtime
		Invoke("SpawnObstacle", 0f);
	}


	void SpawnObstacle() {
		if (GameManager.instance.gameState == GameState.Playing) {
			GameObject obstacle;

			// initially, only regular obstacles will be spawned
			if (GameManager.instance.score < pointsToSecondStage) {
				obstacle = obstacles[0];
			}

			// and further on, just for fun, new obstacle types may spawn as well
			else {
				obstacle = obstacles[Random.Range(0, obstacles.Length)];
			}
			
			float height = Random.Range(-obstaclesHeightVariance, obstaclesHeightVariance);
			Vector3 pos = new Vector3(this.transform.position.x, height, 0f);
			
			GameObject obstacleObj = Instantiate(obstacle, pos, Quaternion.identity) as GameObject;
			obstacleObj.transform.parent = container.transform;
		}

		float nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
		Invoke("SpawnObstacle", nextSpawn);
	}
}
