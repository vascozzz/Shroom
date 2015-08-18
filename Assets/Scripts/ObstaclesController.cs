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
	
	void Start() {
		// invoke the spawn function, which will then invoke itself continuously overtime
		Invoke("SpawnObstacle", 0f);
	}
	
	void SpawnObstacle() {
		if (GameManager.instance.gameState == GameState.Playing) {
			GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];
			
			float height = Random.Range(-obstaclesHeightVariance, obstaclesHeightVariance);
			Vector3 pos = new Vector3(this.transform.position.x, height, 0f);
			
			GameObject obstacleObj = Instantiate(obstacle, pos, Quaternion.identity) as GameObject;
			obstacleObj.transform.parent = container.transform;
		}

		float nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
		Invoke("SpawnObstacle", nextSpawn);
	}
}
