using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerScoreManager : MonoBehaviour
{
	[SerializeField] Slider scoreSlider;
	[SerializeField] Image image;
	public static PlayerScoreManager Instance { get; protected set; }
	[Min(0)]
	[SerializeField] int startingScore = 80;
	[SerializeField] int maxScore = 100;

	[SerializeField] AnimationCurve timeScore;
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
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);

		currentScore = startingScore;
		scoreSlider.maxValue = maxScore;
		scoreSlider.value = currentScore;
		ReEvaluateScoreRank(ref currentRank);
		image.sprite = RanksSprites.GetRankSprite(CurrentRank);



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
		image.sprite = RanksSprites.GetRankSprite(CurrentRank);
	}
	public void ScoreUp()
	{
		currentScore++;

		if (currentScore > maxScore) currentScore = maxScore;

		ReEvaluateScoreRank(ref currentRank);
		image.sprite = RanksSprites.GetRankSprite(CurrentRank);
	}
	public int EvaluateScore(Ranks Rank, float TotalTimeSeconds)
	{
		Debug.Log($"R-{(int)Rank} T-{(int)timeScore.Evaluate(TotalTimeSeconds)} = {(int)Rank + (int)timeScore.Evaluate(TotalTimeSeconds)}");
		return (int)Rank + (int)timeScore.Evaluate(TotalTimeSeconds);
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
}