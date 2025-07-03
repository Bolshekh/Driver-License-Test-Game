using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RanksSprites : MonoBehaviour
{
	[Header("Ranks Sprites")]

	[SerializeField] Sprite rankS;
	[SerializeField] Sprite rankA;
	[SerializeField] Sprite rankB;
	[SerializeField] Sprite rankC;
	[SerializeField] Sprite rankD;

	public static RanksSprites Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
	}

	public static Sprite GetRankSprite(Ranks currentRank)
	{
		return currentRank switch
		{
			Ranks.S => Instance.rankS,
			Ranks.A => Instance.rankA,
			Ranks.B => Instance.rankB,
			Ranks.C => Instance.rankC,
			Ranks.D => Instance.rankD,
			_ => Instance.rankD,
		};
	}
}
