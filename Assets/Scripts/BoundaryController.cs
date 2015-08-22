using UnityEngine;
using System.Collections;

/**
 * Boundary Controller
 * Responsible for destroying all objects in a given layer on touch.
 * Used for destroying obstacles left behind the player.
 */
[RequireComponent(typeof (BoxCollider2D))]
public class BoundaryController : MonoBehaviour 
{	
	public int boundedLayer = 0;


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == boundedLayer) {
			Destroy(other.transform.parent.gameObject);
		}
	}
}
