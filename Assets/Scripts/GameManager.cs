using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance = null;

	public GameState gameState;
	public float respawnDelay = 1f;

	public GameObject UIIntro;
	public GameObject UIInGame;
	public Text UIScore;

	private int score = 0;
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
	}


	public void GameStart() {
		UIIntro.SetActive(false);
		UIInGame.SetActive(true);
		gameState = GameState.Playing;
	}


	public void GameOver() {
		gameState = GameState.GameOver;
		Invoke("ReloadScene", respawnDelay);
	}


	public void UpdateScore() {
		score++;
		UIScore.text = score.ToString();
	}


	void ReloadScene() {
		Application.LoadLevel(0);
	}


	void OnDestroy() {
		if (score > highscore) {
			PlayerPrefs.SetInt("highscore", highscore);
		}
	}
}
