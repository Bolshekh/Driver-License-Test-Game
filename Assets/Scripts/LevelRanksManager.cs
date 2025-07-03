

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelRanksManager
{
	public static int CurrentLevel { get; set; } = 2;
	public static List<Level> Levels { get; private set; } = new List<Level>();

	public static event EventHandler OnSavesLoaded;
	public static void NewScore(Level Level)
	{
		if (Levels.Where(l => l.BuildIndex == Level.BuildIndex).Count() > 0)
		{
			Level _level = Levels.Find(l => l.BuildIndex == Level.BuildIndex);
			if (Level.BestScore > _level.BestScore)
				Levels[Levels.IndexOf(_level)] = Level;
		}
		else
		{
			Levels.Add(Level);
		}
		SaveLevels();
	}
	public static void SaveLevels()
	{
		SaveLoadSystem.Instance.SetSavesData(Levels);
	}
	public static Ranks EvaluateHigherRank(Ranks FirstRank, Ranks SecondRank)
	{
		if (((int)FirstRank) <= ((int)SecondRank))
		{
			return FirstRank;
		}
		else
			return SecondRank;
	}
	public static void LoadLevelsData(List<Level> levels)
	{
		if (levels != null)
		{
			Levels = levels;
			OnSavesLoaded?.Invoke(null, EventArgs.Empty);
		}
	}
}
public enum Ranks
{
	S = 100,
	A = 75,
	B = 50,
	C = 25,
	D = 0
}

[System.Serializable]
public struct Level
{
	public int BuildIndex;
	public Ranks BestRank;
	
	public float BestTime;
	public int BestScore;
}