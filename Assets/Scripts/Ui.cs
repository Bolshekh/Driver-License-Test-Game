using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui : MonoBehaviour
{
	Animator animator;
	public void PlayerRankUp()
	{
		if (animator == null) animator = GetComponent<Animator>();

		animator.CrossFade("score", 0, 0);
	}
}
