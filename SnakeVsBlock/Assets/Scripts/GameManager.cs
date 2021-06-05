using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
	Running,
	Menu
}

public class GameManager : MonoBehaviour
{
	Text scoreTxt = null;
	Text highscoreTxt = null;

	GameObject highscoreImg = null;

	int score;
	int highscore;

	Text onLevelFinishedTxt = null;
	Button replayBtn = null;
	Button nextLvlBtn = null;

	Slider completionPercent;
	public GameState State { get; private set; }
	public int Score
	{
		get { return score; }
		private set 
		{
			score = value;
			if (scoreTxt)
			{
				scoreTxt.text = score.ToString();
			}
		}
	}

    void Start()
    {
		scoreTxt = GameObject.Find("ScoreText").GetComponent<Text>();
		Score = 0;

		onLevelFinishedTxt = GameObject.Find("LevelFinishedText").GetComponent<Text>();
		if (onLevelFinishedTxt)
		{
			onLevelFinishedTxt.enabled = false;
		}

		replayBtn = GameObject.Find("ReplayButton").GetComponent<Button>();
		if (replayBtn)
		{
			replayBtn.onClick.AddListener(() => { OnReplayButton(); });
			replayBtn.gameObject.SetActive(false);
		}
		
		nextLvlBtn = GameObject.Find("NextLevelButton").GetComponent<Button>();
		if (nextLvlBtn)
		{
			nextLvlBtn.onClick.AddListener(() => { OnNextLevel(); });
			nextLvlBtn.gameObject.SetActive(false);
		}

		completionPercent = GameObject.Find("CompletionPercent").GetComponent<Slider>();

		State = GameState.Running;

		if (PlayerPrefs.HasKey("highscore"))
		{
			highscore = PlayerPrefs.GetInt("highscore");
		}
		else
		{
			highscore = 0;
		}

		highscoreTxt = GameObject.Find("HighscoreText").GetComponent<Text>();
		highscoreTxt.text = highscore.ToString();
		highscoreTxt.enabled = false;

		highscoreImg = GameObject.Find("HighscoreSprite");
		highscoreImg.SetActive(false);
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("highscore", highscore);
	}

	public void UpdateCompletionPercent(float value)
	{
		completionPercent.value = Mathf.Clamp01(value);
	}
	public void OnHeadLost()
	{
		Score++;
	}
	public void GameOver()
	{
		GameFinished("Game Over");
	}

	public void LevelCompleted()
	{
		GameFinished("Level Completed!");
	}

	void GameFinished(string msg)
	{
		if (onLevelFinishedTxt && replayBtn && highscoreTxt && highscoreImg)
		{
			onLevelFinishedTxt.enabled = true;
			onLevelFinishedTxt.text = msg;

			highscoreTxt.enabled = true;
			highscoreImg.SetActive(true);

			replayBtn.gameObject.SetActive(true);
			nextLvlBtn.gameObject.SetActive(true);
		}

		if (score > highscore)
		{
			highscore = score;
			highscoreTxt.text = highscore.ToString();
		}

		State = GameState.Menu;
	}

	public void OnReplayButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnNextLevel()
	{
		string crtSceneName = SceneManager.GetActiveScene().name;

		if (crtSceneName == "Level1")
		{
			SceneManager.LoadScene("Level2");
		}
		else
		{
			SceneManager.LoadScene("Level1");
		}
	}
}
