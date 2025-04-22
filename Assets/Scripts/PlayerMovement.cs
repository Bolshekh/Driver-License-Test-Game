using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
	[SerializeField] float bumpCooldown = 3f;
	[SerializeField] BoxCollider2D scoreTrigger;

	bool startedTrick = false;
	[Min(0)]
	float bumpTimeout;

	public UnityEvent OnPlayerCollision;
	public UnityEvent OnPlayerTrick;
	// Start is called before the first frame update
	void Start()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
		trailParticle = trail.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
	{
		var _hor = Input.GetAxis("Horizontal");

		MovementAndRotation(_hor);

		var _angle = Mathf.Abs(Vector2.SignedAngle(transform.up, (Vector3)playerRigidBody.velocity));

		Angle(_angle);

		bumpTimeout -= Time.deltaTime;

		DebugRays();
	}
	void MovementAndRotation(float Axis)
	{
		transform.Rotate(0, 0, rotationSpeed * Axis * (-1) * Time.deltaTime);

		playerRigidBody.AddForce(moveSpeed * transform.up, ForceMode2D.Force);

		trail.transform.rotation = Quaternion.LookRotation(transform.forward, transform.position - (Vector3)playerRigidBody.velocity);
	}
	void Angle(float Angle)
	{
		if (Angle > angleTreshhold)
		{
			scoreTrigger.enabled = true;
			var _em = trailParticle.emission;
			_em.rateOverTime = trailParticleRateOverTime;
			startedTrick = true;
		}
		else
		{
			scoreTrigger.enabled = false;
			var _em = trailParticle.emission;
			_em.rateOverTime = 0;
			startedTrick = false;
		}
	}
	void DebugRays()
	{
		Debug.DrawRay(transform.position, transform.up, Color.magenta, 0.5f);
		Debug.DrawRay(transform.position, Vector3.Normalize((Vector3)playerRigidBody.velocity), Color.yellow, 0.5f);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		playerRigidBody.velocity = new Vector2(0, 0);

		playerRigidBody.AddForce(forceOnContact * collision.GetContact(0).normal, ForceMode2D.Impulse);

		OnPlayerCollision?.Invoke();

		startedTrick = false;

		bumpTimeout = bumpCooldown;
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (startedTrick && bumpTimeout <= 0) OnPlayerTrick?.Invoke();
	}
}
