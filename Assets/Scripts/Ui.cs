using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using DG.Tweening;

public class Ui : MonoBehaviour
{
	[SerializeField] GameObject levelSelectorContents;
	Animator animator;
	[SerializeField] TMP_Text timerText;
	[SerializeField] GameObject rank;

	[Header("RankScale")]

	[SerializeField] float endScale;
	[SerializeField] float duration;
	[SerializeField] float elastic;
	[SerializeField] int vibrato;
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
		if (levelSelectorContents == null) return;

		foreach (Level level in LevelRanksManager.Levels)
		{
			foreach (Transform transform in levelSelectorContents.transform.GetChild(level.BuildIndex - 2))
			{
				if (transform.CompareTag("UiRank"))
				{
					transform.GetComponent<Image>().sprite = PlayerScoreManager.GetRankSprite(level.BestRank);
					transform.gameObject.SetActive(true);
				}
				if (transform.CompareTag("UiTime"))
				{
					transform.GetComponent<TMP_Text>().text = level.BestTime.ToString(@"mm\:ss\:ff");
					transform.gameObject.SetActive(true);
				}
			}
		}
	}
	public void ShowAd()
	{
		YG2.InterstitialAdvShow();

	}
}
