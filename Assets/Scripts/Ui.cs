using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
public class Ui : MonoBehaviour
{
	[SerializeField] GameObject levelSelectorContents;
	Animator animator;
	[SerializeField] TMP_Text timerText;
	[SerializeField] Texture2D cursorTexture;
	[SerializeField] Vector2 cursorHotspot;
	float levelStart;
	static public float TimeTotal;
	public void PlayerRankUp()
	{
		if (animator == null) animator = GetComponent<Animator>();

		animator.CrossFade("score", 0, 0);
	}
	private void Update()
	{
		if (timerText == null) return;
		Debug.Log(Time.time - levelStart);
		TimeTotal = Time.time - levelStart;
		timerText.text = System.TimeSpan.FromSeconds(TimeTotal).ToString(@"mm\:ss\:fff");
		//timerText.text = new DateTime(TimeSpan.FromSeconds(Time.time - levelStart).Ticks).ToString("mm:ss:ff");
	}
	private void Awake()
	{
		//Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);
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
