using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxController : MonoBehaviour 
{
	public float horizontalSpeed;
	public int sideCopies;
	public float boundary;

	private BoxCollider2D boxCollider;
	private SpriteRenderer sRenderer;
	private float spriteWidth;

	private List<GameObject> copies;

	
	void Start() {
		copies = new List<GameObject>();

		sRenderer = this.GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.bounds.size.x - 0.1f;

		sRenderer.enabled = false;

		// tile the background with multiples copies, beginning with the left
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
		copies.Add (copy);
	}


	void FixedUpdate() {
		if (GameManager.instance.gameState == GameState.Playing) {
			foreach (GameObject copy in copies) {
				copy.transform.localPosition += new Vector3(Time.deltaTime * horizontalSpeed, 0f, 0f);
				
				if (copy.transform.localPosition.x <= boundary) {
					Vector3 pos = copy.transform.position;
					pos.x += spriteWidth * (sideCopies * 2 + 1);
					
					copy.transform.position = pos;
				}
			}
		}
	}
}
