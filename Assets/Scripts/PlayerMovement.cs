using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float rotationSpeed;
	[SerializeField] float moveSpeed;
	Rigidbody2D playerRigidBody;
	[SerializeField] GameObject trail;
	ParticleSystem trailParticle;
	[SerializeField] float trailParticleRateOverTime = 10f;
	[SerializeField] float angleTreshhold = 75;
	[SerializeField] float forceOnContact = 10f;
	// Start is called before the first frame update
	void Start()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
		trailParticle = trail.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
	{
		var hor = Input.GetAxis("Horizontal");

		//transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

		transform.Rotate(0, 0, rotationSpeed * hor * (-1) * Time.deltaTime);
		//transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.up, transform.up - (Time.deltaTime * hor), Time.deltaTime, 0)));


		playerRigidBody.AddForce(moveSpeed * transform.up, ForceMode2D.Force);

		trail.transform.rotation = Quaternion.LookRotation( transform.forward, transform.position - (Vector3)playerRigidBody.velocity);

		var _angle = Mathf.Abs(Vector2.SignedAngle(-transform.up, transform.position - (Vector3)playerRigidBody.velocity));

		if (_angle > angleTreshhold)
		{
			var _em = trailParticle.emission;
			_em.rateOverTime = trailParticleRateOverTime;
		}
		else
		{
			var _em = trailParticle.emission;
			_em.rateOverTime = 0;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		playerRigidBody.velocity = new Vector2(0,0);

		playerRigidBody.AddForce(forceOnContact * collision.GetContact(0).normal, ForceMode2D.Impulse);
	}
}
