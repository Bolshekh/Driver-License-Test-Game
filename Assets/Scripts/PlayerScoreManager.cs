using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerScoreManager : MonoBehaviour
{
	[SerializeField] Slider scoreSlider;
	[SerializeField] Image image;
	[SerializeField] Sprite rankS;
	[SerializeField] Sprite rankA;
	[SerializeField] Sprite rankB;
	[SerializeField] Sprite rankC;
	[SerializeField] Sprite rankD;
	public static PlayerScoreManager Instance { get; protected set; }
	[Min(0)]
	[SerializeField] int startingScore = 80;
	[SerializeField] int maxScore = 100;
	int currentScore;
	Ranks currentRank;
	public Ranks CurrentRank 
	{
		get => currentRank;
		private set => currentRank = value;
	}
	Ranks prevRank;

	[SerializeField] float sliderSmooth = 30f;
	float vel;

	public UnityEvent OnPlayerRankUp;
	// Start is called before the first frame update
	void Awake()
	{
		Instance = this;

		currentScore = startingScore;
		scoreSlider.maxValue = maxScore;
		scoreSlider.value = currentScore;
		ReEvaluateScoreRank(ref currentRank);
		image.sprite = GetRankSprite(CurrentRank);



		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
	}
	private void Update()
	{
		scoreSlider.value = Mathf.SmoothDamp(scoreSlider.value, currentScore, ref vel, sliderSmooth * Time.deltaTime);
	}
	public void ScoreDown()
	{
		currentScore--;
		
		if (currentScore < 0) currentScore = 0;

		ReEvaluateScoreRank(ref currentRank);
		image.sprite = GetRankSprite(CurrentRank);
	}
	public void ScoreUp()
	{
		currentScore++;

		if (currentScore > maxScore) currentScore = maxScore;

		ReEvaluateScoreRank(ref currentRank);
		image.sprite = GetRankSprite(CurrentRank);
	}
	void ReEvaluateScoreRank(ref Ranks curRank)
	{
		if (currentScore > maxScore * 0.8) curRank = Ranks.S;
		else if (currentScore > maxScore * 0.6) curRank = Ranks.A;
		else if (currentScore > maxScore * 0.4) curRank = Ranks.B;
		else if (currentScore > maxScore * 0.2) curRank = Ranks.C;
		else curRank = Ranks.D;

		if (prevRank != currentRank)
		{
			prevRank = currentRank;
			RankChange();
		}
	}
	void RankChange()
	{
		OnPlayerRankUp?.Invoke();
	}
	public static Sprite GetRankSprite(Ranks currentRank) => currentRank switch
	{
		Ranks.S => Instance.rankS,
		Ranks.A => Instance.rankA,
		Ranks.B => Instance.rankB,
		Ranks.C => Instance.rankC,
		Ranks.D => Instance.rankD,
		_ => Instance.rankD,
	};
}