

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelRanksManager
{
	public static int CurrentLevel { get; set; } = 2;
	public static List<Level> Levels { get; private set; } = new List<Level>();
	public static void NewScore(Level Level)
	{
		if (Levels.Where(l => l.BuildIndex == Level.BuildIndex).Count() > 0)
		{
			Level _level = Levels.Find(l => l.BuildIndex == Level.BuildIndex);
			if (Level.BestScore > _level.BestScore)
				Levels[Levels.IndexOf(_level)] = Level;
				//{
				//	BuildIndex = Level.BuildIndex,
				//	BestRank = Level.BestRank,
				//	BestTime = Level.BestTime,
				//	BestScore = 
				//};
		}
		else
		{
			Levels.Add(Level);
		}
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
			Levels = levels;
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

public struct Level
{
	public int BuildIndex { get; set; }
	public Ranks BestRank { get; set; }
	public TimeSpan BestTime { get; set; }
	public int BestScore { get; set; }
}