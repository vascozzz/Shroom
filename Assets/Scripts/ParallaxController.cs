using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Parallax Controller
 * Responsible for moving a background/foreground element, generating a parallax effect.
 * Works by texture offseting.
 */
[RequireComponent(typeof(Renderer))]
public class ParallaxController : MonoBehaviour 
{
	[Range(0f, 1f)] public float speed = 0.5f;

	private Renderer tRenderer;


	void Start() {
		tRenderer = this.GetComponent<Renderer>();
	}


	void Update() {
		if (GameManager.instance.gameState != GameState.GameOver) {
			tRenderer.material.mainTextureOffset = new Vector2((Time.time * speed)%1, 0f);
		}
	}
}
