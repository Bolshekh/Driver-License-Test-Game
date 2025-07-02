using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveLoadSystem : MonoBehaviour
{
	public static SaveLoadSystem Instance { get; set; }
	void Start()
	{
		YG2.onGetSDKData += GetSavesData;

		if (SaveLoadSystem.Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}
	private void OnEnable()
	{
		YG2.onGetSDKData += GetSavesData;
	}

	private void OnDisable()
	{
		YG2.onGetSDKData -= GetSavesData;
	}

	public void GetSavesData()
	{
		LevelRanksManager.LoadLevelsData(YG2.saves.Levels);
		Debug.Log(YG2.saves.Levels.Count);
	}

	public void SetSavesData(List<Level> Levels)
	{
		YG2.saves.Levels = LevelRanksManager.Levels;

		YG2.SaveProgress();
	}
}

namespace YG
{
	public partial class SavesYG
	{
		public List<Level> Levels { get; set; }
	}
}