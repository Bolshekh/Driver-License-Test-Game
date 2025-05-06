using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField] GameObject menuUi;
	[SerializeField] AudioMixer audioMixer;

	public int CurrentLevel { get; set; } = 2;
	public void SwitchMenu()
	{
		if (menuUi.activeSelf)
		{
			CloseMenu();
		}
		else
		{
			OpenMenu();
		}
	}
	public void OpenMenu()
	{
		menuUi.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0f;
		audioMixer.SetFloat("CarVolume", -80);
	}
	public void CloseMenu()
	{
		menuUi.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1f;
		audioMixer.SetFloat("CarVolume", 0);
	}
	public void Exit()
	{
		Application.Quit();
	}
	public void MainMenu()
	{
		CloseMenu();
		SceneManager.LoadScene(0);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	public void ReloadCurrentScene()
	{
		CloseMenu();
		SceneManager.LoadSceneAsync(1);
		SceneManager.LoadSceneAsync(CurrentLevel, LoadSceneMode.Additive);
	}
	public void LoadNextScene()
	{
		CloseMenu();
		SceneManager.LoadSceneAsync(1);
		SceneManager.LoadSceneAsync(CurrentLevel + 1, LoadSceneMode.Additive);
		CurrentLevel++;
	}
	public void LoadScene(int SceneIndex)
	{
		CloseMenu();
		SceneManager.LoadSceneAsync(1);
		SceneManager.LoadSceneAsync(SceneIndex, LoadSceneMode.Additive);
	}
	public void LoadScene(string SceneName)
	{
		CloseMenu();
		SceneManager.LoadScene(SceneName);
	}
}
