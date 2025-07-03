using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		if(YG2.saves.Levels != null)
		LevelRanksManager.LoadLevelsData(YG2.saves.Levels.ToList());

		LogLevelInfo();
	}

	public void SetSavesData(List<Level> Levels)
	{
		YG2.saves.Levels = Levels.ToArray();

		YG2.SaveProgress();

		LogLevelInfo();
	}
	public static void LogLevelInfo()
	{
		if (YG2.saves.Levels == null)
		{
			Debug.LogError("levels loading failed, YG2.saves.Levels is null");
			return;
		}
		Debug.Log($"Saved Levels Count: {YG2.saves.Levels.Count()}");
		foreach (Level l in YG2.saves.Levels)
			Debug.Log($"index:{l.BuildIndex};time:{l.BestTime};rank:{l.BestRank};score:{l.BestScore}");
	}
}

namespace YG
{
	public partial class SavesYG
	{
		public Level[] Levels;
	}
}