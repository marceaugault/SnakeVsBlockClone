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

	int score;

	Text onLevelFinishedTxt = null;
	Button replayBtn = null;

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

		completionPercent = GameObject.Find("CompletionPercent").GetComponent<Slider>();

		State = GameState.Running;
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
		if (onLevelFinishedTxt && replayBtn)
		{
			onLevelFinishedTxt.enabled = true;
			onLevelFinishedTxt.text = "Game Over";

			replayBtn.gameObject.SetActive(true);
		}

		State = GameState.Menu;
	}

	public void LevelCompleted()
	{
		if (onLevelFinishedTxt && replayBtn)
		{
			onLevelFinishedTxt.enabled = true;
			onLevelFinishedTxt.text = "Level Completed!";

			replayBtn.gameObject.SetActive(true);
		}

		State = GameState.Menu;
	}

	public void OnReplayButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
