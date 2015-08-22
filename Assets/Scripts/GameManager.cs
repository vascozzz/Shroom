using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Game Manager
 * Responsible for controlling game state and events (including scoring system, UI). 
 * Maintains a static instance available to all other scripts.
 */
public class GameManager : MonoBehaviour 
{
	public static GameManager instance = null;

	// game state
	[HideInInspector] public GameState gameState;
	public float respawnDelay = 1f;
	private float gameOverTime = 0;

	// ui state
	public CanvasScaler UICanvas;
	public GameObject UIIntro;
	public GameObject UIPlaying;
	public GameObject UIGameOver;

	// ui score
	public Text UIPlayingScore;
	public Text UIGameOverScore;
	public Text UIGameOverHighscore;

	// scoring system
	[HideInInspector] public int score = 0;
	private int highscore;


	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance != null) {
			Destroy(this.gameObject);
		}
	}


	void Start() {
		highscore = PlayerPrefs.GetInt("highscore", 0);
		switchState(GameState.Intro);
	}


	public void GameStart() {
		switchState(GameState.Playing);
	}


	public void GameOver() {
		switchState(GameState.GameOver);

		UIGameOverScore.text = "Score: " + score.ToString();
		UIGameOverHighscore.text = "Best: " + highscore.ToString();

		gameOverTime = Time.time;
	}


	public void UpdateScore() {
		score++;
		UIPlayingScore.text = score.ToString();
	}


	public void ResetGame() {
		// player will only be allowed to respawn after a certain delay
		if (Time.time > gameOverTime + respawnDelay) {
			Application.LoadLevel(0);
		}
	}


	private void switchState(GameState newState) {
		gameState = newState;

		switch (gameState) {
			case GameState.Intro:
				UIIntro.SetActive(true);
				UIPlaying.SetActive(false);
				UIGameOver.SetActive(false);
				break;

			case GameState.Playing:
				UIIntro.SetActive(false);
				UIGameOver.SetActive(false);
				UICanvas.matchWidthOrHeight = 0.5f;
				UIPlaying.SetActive(true);
				break;

			case GameState.GameOver:
				UIIntro.SetActive(false);
				UIPlaying.SetActive(false);
				UIGameOver.SetActive(true);
				break;

			default: 
				break;
		}
	}


	void OnDestroy() {
		if (score > highscore) {
			PlayerPrefs.SetInt("highscore", score);
		}
	}
}
