using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
	//movement
	[Header("Movement")]
	[SerializeField] float rotationSpeed;
	float rotationMultiplier = 1f;
	[SerializeField] float moveSpeed;
	float speedMultiplier = 1f;
	Rigidbody2D playerRigidBody;
	[SerializeField] float angleTreshhold = 75;
	[SerializeField] float forceOnContact = 10f;
	float uiInput;

	//effects
	[Header("Effects")]
	[SerializeField] List<GameObject> trail = new List<GameObject>();
	readonly List<ParticleSystem> trailParticle = new List<ParticleSystem>();
	[SerializeField] float trailParticleRateOverTime = 10f;
	[SerializeField] List<TrailRenderer> tracks = new List<TrailRenderer>();

	//score
	[Header("Score")]
	bool startedTrick = false;
	[Min(0)]
	float bumpTimeout;
	[SerializeField] float bumpCooldown = 3f;
	[SerializeField] BoxCollider2D scoreTrigger;

	//audio
	[Header("Audio")]
	[SerializeField] AudioSource engineAudio;
	[Range(0,100)]
	[SerializeField] float audioPitch = 20f;
	float engineAudioVel;
	[SerializeField] float audioSmooth = 20;

	[SerializeField] AudioSource crashAudio;

	[SerializeField] AudioSource skidAudio;
	float skidAudioVel;
	float skidPitchAudioVel;
	[SerializeField] float skidAudioSmooth = 1;
	bool isDrifting = false;

	//events
	[Header("Events")]
	public UnityEvent OnPlayerCollision;
	public UnityEvent OnPlayerTrick;
	public UnityEvent OnPlayerWon;
	public UnityEvent OnPlayerEscapeButton;
	// Start is called before the first frame update
	void Start()
	{
		speedMultiplier = 1;
		rotationMultiplier = 1;

		playerRigidBody = GetComponent<Rigidbody2D>();
		trail.ForEach(t => trailParticle.Add(t.GetComponent<ParticleSystem>()));
		StopEmit();
	}

	// Update is called once per frame
	void Update()
	{

		var _angle = Mathf.Abs(Vector2.SignedAngle(transform.up, (Vector3)playerRigidBody.velocity));

		Angle(_angle);

		OtherInput();

		bumpTimeout -= Time.deltaTime;

		ChangeAudioPitch(0.01f * audioPitch * (Vector3.Distance(Vector3.zero, 0.1f * playerRigidBody.velocity) - 0.5f));
		ChangeAudioVolume(isDrifting);

		DebugRays();
	}
	private void FixedUpdate()
	{
		var _hor = Mathf.Clamp(Input.GetAxis("Horizontal") + uiInput, -1, 1);

		MovementAndRotation(_hor);

	}
	public void AddUiInput(float Axis)
	{
		uiInput = Axis;
	}
	void MovementAndRotation(float Axis)
	{
		transform.Rotate(0, 0, rotationSpeed * rotationMultiplier * Axis * (-1) * Time.deltaTime);

		playerRigidBody.AddForce(moveSpeed * speedMultiplier * Time.deltaTime * transform.up, ForceMode2D.Force);

		trail.ForEach(
			t=>t.transform.rotation = Quaternion.LookRotation(transform.forward, transform.position - (Vector3)playerRigidBody.velocity)
		);
	}
	void Angle(float Angle)
	{
		if (Angle > angleTreshhold)
		{
			scoreTrigger.enabled = true;
			Emit();
			startedTrick = true;
			isDrifting = true;
		}
		else
		{
			scoreTrigger.enabled = false;
			StopEmit();
			startedTrick = false;
			isDrifting = false;
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
	void ChangeAudioPitch(float Value)
	{
		engineAudio.pitch = Mathf.SmoothDamp(engineAudio.pitch, 1 + Value, ref engineAudioVel, audioSmooth);
		skidAudio.pitch = Mathf.SmoothDamp(skidAudio.pitch, 0.7f + Value, ref skidPitchAudioVel, audioSmooth);
	}
	void ChangeAudioVolume(bool isDrifting)
	{
		skidAudio.volume = Mathf.Clamp(Mathf.SmoothDamp(skidAudio.volume, isDrifting ? 1 : 0, ref skidAudioVel, skidAudioSmooth), 0, 0.3f);
	}
	void OtherInput()
	{
		if(Input.GetButtonDown("Cancel"))
		{
			OnPlayerEscapeButton?.Invoke();
		}
	}
	public void PlayCrashSound()
	{
		crashAudio.pitch = 1 + UnityEngine.Random.Range(-audioPitch * 0.005f, audioPitch * 0.005f);
		crashAudio.Play();
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		playerRigidBody.velocity = new Vector2(0, 0);

		playerRigidBody.AddForce(forceOnContact * collision.GetContact(0).normal, ForceMode2D.Impulse);

		OnPlayerCollision?.Invoke();

		startedTrick = false;

		bumpTimeout = bumpCooldown;

		if (collision.collider.gameObject.CompareTag("Obstacle"))
			collision.collider.gameObject.GetComponent<Cone>().Hit();
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (startedTrick && bumpTimeout <= 0) OnPlayerTrick?.Invoke();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("WinTrigger"))
		{
			speedMultiplier = 0;
			rotationMultiplier = 0;
			OnPlayerWon?.Invoke();
		}
	}
}
