using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Parallax Controller
 * Responsible for moving a background element, generating a parallax effect.
 * Works by creating multiples tiled copies of the original element that are all moved simultaneously. 
 * Once a copy reaches a set limit position, it is placed behind the other copies and resumes its movement.
 */
public class ParallaxController : MonoBehaviour 
{
	public float horizontalSpeed; // negative values move the element to the left
	public int sideCopies; // number of copies of the original element 
	public float boundary; // limit position which resets a copy

	private BoxCollider2D boxCollider;
	private SpriteRenderer sRenderer;
	private float spriteWidth;

	private List<GameObject> copies;

	
	void Start() {
		copies = new List<GameObject>();

		// disable current renderer, everything will be handled by the copies
		sRenderer = this.GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.bounds.size.x;
		spriteWidth -= 0.1f; // smal overlap to fix pixel gap imperfections
		sRenderer.enabled = false;

		// tiled copies, beginning with the left
		for (int i = sideCopies; i > 0; i--) {
			createCopy(-i * spriteWidth);
		}

		// right-side and center copies
		for (int i = 0; i <= sideCopies; i++) {
			createCopy(i * spriteWidth);
		}
	}


	void createCopy(float pos) {
		GameObject copy = new GameObject();
		copy.name = this.name + "Copy";

		// set new position
		copy.transform.parent = this.transform;
		copy.transform.localPosition = new Vector3(pos, 0f, 0f);

		// copy sprite renderer
		copy.AddComponent<SpriteRenderer>();
		SpriteRenderer copyRenderer = copy.GetComponent<SpriteRenderer>();
		copyRenderer.sprite = sRenderer.sprite;
		copyRenderer.sortingOrder = sRenderer.sortingOrder;

		// add to list, so they can be updated individually
		copies.Add(copy);
	}


	void FixedUpdate() {
		if (GameManager.instance.gameState == GameState.Playing) {
			// move all sprites simultaneously
			foreach (GameObject copy in copies) {
				copy.transform.localPosition += new Vector3(Time.deltaTime * horizontalSpeed, 0f, 0f);
				
				// once the boundary is reached, copies are placed behind all the others
				if (copy.transform.localPosition.x <= boundary) {
					Vector3 pos = copy.transform.position;
					pos.x += spriteWidth * (sideCopies * 2 + 1);
					
					copy.transform.position = pos;
				}
			}
		}
	}
}
