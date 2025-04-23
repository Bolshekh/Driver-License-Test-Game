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
	[SerializeField] List<GameObject> trail = new List<GameObject>();
	List<ParticleSystem> trailParticle = new List<ParticleSystem>();
	[SerializeField] float trailParticleRateOverTime = 10f;
	[SerializeField] float angleTreshhold = 75;
	[SerializeField] float forceOnContact = 10f;
	[SerializeField] float bumpCooldown = 3f;
	[SerializeField] BoxCollider2D scoreTrigger;
	[SerializeField] List<TrailRenderer> tracks = new List<TrailRenderer>();

	bool startedTrick = false;
	[Min(0)]
	float bumpTimeout;

	public UnityEvent OnPlayerCollision;
	public UnityEvent OnPlayerTrick;
	// Start is called before the first frame update
	void Start()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
		trail.ForEach(t => trailParticle.Add(t.GetComponent<ParticleSystem>()));
		StopEmit();
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

		trail.ForEach(t=> t.transform.rotation = Quaternion.LookRotation(transform.forward, transform.position - (Vector3)playerRigidBody.velocity));
	}
	void Angle(float Angle)
	{
		if (Angle > angleTreshhold)
		{
			scoreTrigger.enabled = true;
			Emit();
			startedTrick = true;
		}
		else
		{
			scoreTrigger.enabled = false;
			StopEmit();
			startedTrick = false;
		}
	}
	void Emit()
	{
		trailParticle.ForEach(t =>
		{
			var _em = t.emission;
			_em.rateOverTime = trailParticleRateOverTime;
		});
		tracks.ForEach(t => t.emitting = true);
	}
	void StopEmit()
	{
		trailParticle.ForEach(t =>
		{
			var _em = t.emission;
			_em.rateOverTime = 0;
		});
		tracks.ForEach(t => t.emitting = false);
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
