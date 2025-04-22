using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	[Min(0)]
	[SerializeField] int startingScore = 80;
	[SerializeField] int maxScore = 100;
	int currentScore;
	Ranks currentRank;
	// Start is called before the first frame update
	void Start()
	{
		currentScore = startingScore;
		scoreSlider.maxValue = maxScore;
		scoreSlider.value = currentScore;
		ReEvaluateScoreRank(ref currentRank);
		UpdateUi();
	}
	public void ScoreDown()
	{
		currentScore--;
		
		if (currentScore < 0) currentScore = 0;

		ReEvaluateScoreRank(ref currentRank);
		UpdateUi();
	}
	public void ScoreUp()
	{
		currentScore++;

		if (currentScore > maxScore) currentScore = maxScore;

		ReEvaluateScoreRank(ref currentRank);
		UpdateUi();
	}
	void ReEvaluateScoreRank(ref Ranks curRank)
	{
		if (currentScore > maxScore * 0.8) curRank = Ranks.S;
		else if (currentScore > maxScore * 0.6) curRank = Ranks.A;
		else if (currentScore > maxScore * 0.4) curRank = Ranks.B;
		else if (currentScore > maxScore * 0.2) curRank = Ranks.C;
		else curRank = Ranks.D;
	}
	void UpdateUi()
	{
		scoreSlider.value = currentScore;
		switch (currentRank)
		{
			case Ranks.S:
				image.sprite = rankS;
				break;
			case Ranks.A:
				image.sprite = rankA;
				break;
			case Ranks.B:
				image.sprite = rankB;
				break;
			case Ranks.C:
				image.sprite = rankC;
				break;
			case Ranks.D:
				image.sprite = rankD;
				break;
		}
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
