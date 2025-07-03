using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using DG.Tweening;
using System.Runtime.InteropServices;
using System;

public class Ui : MonoBehaviour
{
	[SerializeField] GameObject levelSelectorContents;
	Animator animator;
	[SerializeField] TMP_Text timerText;
	[SerializeField] GameObject rank;
	[SerializeField] TMP_Text lang;

	[Header("RankScale")]

	[SerializeField] float endScale;
	[SerializeField] float duration;
	[SerializeField] float elastic;
	[SerializeField] int vibrato;

	[Header("Ranks Sprites")]

	[SerializeField] Sprite rankS;
	[SerializeField] Sprite rankA;
	[SerializeField] Sprite rankB;
	[SerializeField] Sprite rankC;
	[SerializeField] Sprite rankD;

	float levelStart;
	static public float TimeTotal;
	public void PlayerRankUp()
	{
		rank.transform.DOPunchScale(new Vector3(endScale, endScale, endScale), duration, vibrato, elastic).SetEase(Ease.InBounce).SetLoops(1, LoopType.Yoyo).Play();
	}
	private void Update()
	{
		if (timerText == null) return;

		TimeTotal = Time.time - levelStart;
		timerText.text = System.TimeSpan.FromSeconds(TimeTotal).ToString(@"mm\:ss\:fff");
	}
	private void Awake()
	{
		levelStart = Time.time;
		try
		{
			lang.text = GetLang();
		}
		catch
		{

		}

		if (levelSelectorContents == null) return;

		Debug.Log("ui awake");
		LoadLevelsInfo();

		LevelRanksManager.OnSavesLoaded += (s, e) =>
		{
			Debug.Log("saves load event");
			LoadLevelsInfo();
		};
	}
	public void LoadLevelsInfo()
	{
		SaveLoadSystem.LogLevelInfo();

		foreach (Level level in LevelRanksManager.Levels)
		{
			foreach (Transform transform in levelSelectorContents.transform.GetChild(level.BuildIndex - 2))
			{
				if (transform.CompareTag("UiRank"))
				{
					transform.GetComponent<Image>().sprite = RanksSprites.GetRankSprite(level.BestRank);
					transform.gameObject.SetActive(true);
				}
				if (transform.CompareTag("UiTime"))
				{
					transform.GetComponent<TMP_Text>().text = TimeSpan.FromSeconds(level.BestTime).ToString(@"mm\:ss\:ff");
					transform.gameObject.SetActive(true);
				}
			}
		}
	}
	public void ShowAd()
	{
		YG2.InterstitialAdvShow();

	}
	[DllImport("__Internal")]
	private static extern string GetLang();
}
