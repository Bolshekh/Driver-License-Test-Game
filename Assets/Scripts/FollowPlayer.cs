using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	[SerializeField] GameObject player;

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector3(0, Mathf.Clamp(player.transform.position.y, 0, 117), 0);
	}
}
