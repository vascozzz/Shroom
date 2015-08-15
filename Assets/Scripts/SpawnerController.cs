using UnityEngine;
using System.Collections;

public class SpawnerController : MonoBehaviour 
{
	public GameObject[] obstacles;
	public float minSpawnInterval = 1f;
	public float maxSpawnInterval = 2f;
	public float obstaclesHeightVariance = 0.4f;

	
	void Start() {
		Invoke("SpawnObstacle", 0f);
	}

	
	void SpawnObstacle() {
		GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];

		float height = Random.Range(-obstaclesHeightVariance, obstaclesHeightVariance);
		Vector3 pos = new Vector3(this.transform.position.x, height, 0f);

		GameObject obstacleObj = Instantiate(obstacle, pos, Quaternion.identity) as GameObject;

		float nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
		Invoke("SpawnObstacle", nextSpawn);
	}
}
