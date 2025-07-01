using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveLoadSystem : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		YG2.onGetSDKData += GetSavesData;
	}
	private void OnEnable()
	{
		YG2.onGetSDKData += GetSavesData;
	}

	// Отписываемся от ивента onGetSDKData
	private void OnDisable()
	{
		YG2.onGetSDKData -= GetSavesData;
	}

	void GetSavesData()
	{
		LevelRanksManager.LoadLevelsData(YG2.saves.Levels);
	}

	void SetSavesData(List<Level> Levels)
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