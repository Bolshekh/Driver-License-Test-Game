using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] GameObject menuUi;
	[SerializeField] AudioMixer audioMixer;
	[SerializeField] GameObject winUi;
	[SerializeField] Image levelCompletedRank;
	public void SwitchMenu()
	{
		if (menuUi.activeSelf)
		{
			CloseMenu();
		}
		else if (!winUi.activeSelf)
		{
			OpenMenu();
		}
		
	}
	public void OpenMenu()
	{
		menuUi.SetActive(true);
		//Cursor.lockState = CursorLockMode.None;
		//Cursor.visible = true;
		Time.timeScale = 0f;
		audioMixer.SetFloat("CarVolume", -80);
	}
	public void CloseMenu()
	{
		menuUi.SetActive(false);
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
		Time.timeScale = 1f;
		audioMixer.SetFloat("CarVolume", 0);
	}
	public void PlayerWon()
	{
		OpenMenu();
		menuUi.SetActive(false);

		winUi.SetActive(true);
		var score = GetComponent<PlayerScoreManager>();
		levelCompletedRank.sprite = RanksSprites.GetRankSprite(score.CurrentRank);

		LevelRanksManager.NewScore(new Level()
		{
			BuildIndex = LevelRanksManager.CurrentLevel,
			BestRank = score.CurrentRank,
			BestTime = Ui.TimeTotal,
			BestScore = PlayerScoreManager.Instance.EvaluateScore(score.CurrentRank, Ui.TimeTotal)
		});
	}
	public void Exit()
	{
		Application.Quit();
	}
	public void MainMenu()
	{
		CloseMenu();
		SceneManager.LoadScene(0);
		//Cursor.lockState = CursorLockMode.None;
		//Cursor.visible = true;
	}
	public void ReloadCurrentScene()
	{
		CloseMenu();
		SceneManager.LoadScene(1);
		SceneManager.LoadScene(LevelRanksManager.CurrentLevel, LoadSceneMode.Additive);//.completed += (s) => CloseMenu();
	}
	public void LoadNextScene()
	{
		CloseMenu();

		LevelRanksManager.CurrentLevel++;
		if(LevelRanksManager.CurrentLevel > 11)
		{
			LevelRanksManager.CurrentLevel = 2;
		}

		ReloadCurrentScene();
	}
	public void LoadScene(int SceneIndex)
	{
		CloseMenu();
		SceneManager.LoadScene(1);
		SceneManager.LoadScene(SceneIndex, LoadSceneMode.Additive);
		LevelRanksManager.CurrentLevel = SceneIndex;
	}
	public void LoadScene(string SceneName)
	{
		CloseMenu();
		SceneManager.LoadScene(SceneName);
	}
}
