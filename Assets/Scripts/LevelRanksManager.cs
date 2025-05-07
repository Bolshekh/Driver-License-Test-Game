

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelRanksManager
{
	public static int CurrentLevel { get; set; } = 2;
	public static List<Level> Levels { get; } = new List<Level>();
	public static void NewScore(Level Level)
	{
		if (Levels.Where(l => l.BuildIndex == Level.BuildIndex).Count() > 0)
		{
			Level _level = Levels.Find(l => l.BuildIndex == Level.BuildIndex);
			Levels[Levels.IndexOf(_level)] = new Level()
			{
				BuildIndex = Level.BuildIndex,
				BestRank = EvaluateHigherRank(_level.BestRank, Level.BestRank)
			};
		}
		else
		{
			Levels.Add(Level);
		}
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
}
public enum Ranks
{
	S,
	A,
	B,
	C,
	D
}

public struct Level
{
	public int BuildIndex { get; set; }
	public Ranks BestRank { get; set; }
}