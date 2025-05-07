using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
	[SerializeField] GameObject levelSelectorContents;
	Animator animator;
	public void PlayerRankUp()
	{
		if (animator == null) animator = GetComponent<Animator>();

		animator.CrossFade("score", 0, 0);
	}
	private void Awake()
	{
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
			}
		}
	}
}
