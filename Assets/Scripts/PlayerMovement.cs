using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float rotationSpeed;
	[SerializeField] float moveSpeed;
	Rigidbody2D playerRigidBody;
	[SerializeField] GameObject target;
	// Start is called before the first frame update
	void Start()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		var hor = Input.GetAxis("Horizontal");

		//transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

		transform.Rotate(0, 0, rotationSpeed * hor * (-1) * Time.deltaTime);
		//transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.up, transform.up - (Time.deltaTime * hor), Time.deltaTime, 0)));


		playerRigidBody.AddForce(moveSpeed * transform.up, ForceMode2D.Force);

		Debug.DrawRay(transform.position, (Vector3)playerRigidBody.velocity - transform.up, Color.red, 1f);
	}
}
