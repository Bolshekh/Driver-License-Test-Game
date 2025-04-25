using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
	[SerializeField] List<Sprite> hitCones;
	bool isHit = false;
	SpriteRenderer coneSprite;
	private void Start()
	{
		coneSprite = GetComponent<SpriteRenderer>();
	}
	public void Hit()
	{
		if (isHit) return;

		isHit = true;

		coneSprite.sprite = hitCones[Random.Range(0, hitCones.Count)];
	}
}
