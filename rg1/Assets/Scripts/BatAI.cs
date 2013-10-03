﻿using UnityEngine;
using System.Collections;

public class BatAI : MonoBehaviour {

	public AudioClip m_attackAudioClip;
	public Transform m_leftBumper;
	public Transform m_rightBumper;

	public Vector3 m_direction;
	public float m_speed;
	public float m_rotSpeed;
	public float m_rotationFactor;

	private AudioSource m_AudioSource;

	private Vector3 m_targetLoc = new Vector3(-0.956543f, 2.580719f, 1.517822f);

	void OnEnable ()
	{
		m_AudioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
		m_AudioSource.clip = m_attackAudioClip;
	}

	void Start ()
	{
		StartCoroutine(DelayedLook());
	}

	void FixedUpdate ()
	{
		MoveAvoid();
		if (!AvoidRight())
		{
			AvoidLeft();
		}
	}

	void Update ()
	{
		if ((transform.position - m_targetLoc).magnitude < 3.0f)
		{
			if (!m_AudioSource.isPlaying) {
				m_AudioSource.Play();
			}
		} else {
			Debug.Log((transform.position - m_targetLoc).magnitude);
		}
	}

	IEnumerator DelayedLook ()
	{
		while (true) {
			yield return new WaitForSeconds(2.0f);
			AimAtLoc();
		}
	}

	void AimAtLoc () {

		Vector3 loc = m_targetLoc;
		transform.LookAt(loc);
	}

	void MoveAvoid () {
		float speedScale = GetMoveSpeed();
		transform.position += (Time.fixedDeltaTime * speedScale * m_speed) * transform.forward;
	}

	bool AvoidRight ()
	{
		RaycastHit hitInfo;
		bool avoid = Physics.Raycast(transform.position, m_rightBumper.position-transform.position, out hitInfo);
		if (avoid)
		{
			float rotationSlowFactor = 0.0f;
			if (hitInfo.distance != 0.0f) {
				rotationSlowFactor = Mathf.Exp(m_rotationFactor * -hitInfo.distance);
			}
			transform.Rotate(transform.up * rotationSlowFactor * -m_rotSpeed * Time.fixedDeltaTime);
		}
		return avoid;
	}

	bool AvoidLeft ()
	{
		RaycastHit hitInfo;
		bool avoid = Physics.Raycast(transform.position, m_leftBumper.position-transform.position, out hitInfo);
		if (avoid)
		{
			float rotationSlowFactor = 0.0f;
			if (hitInfo.distance != 0.0f) {
				rotationSlowFactor = Mathf.Exp(m_rotationFactor * -hitInfo.distance);
			}
			transform.Rotate(transform.up * rotationSlowFactor * m_rotSpeed * Time.fixedDeltaTime);
		}
		return avoid;
	}

	float GetMoveSpeed ()
	{
		float scale = 1.0f;
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
		{
			scale =  Mathf.Lerp(0.0f, 1.0f, 1.0f - Mathf.Exp(-hitInfo.distance));
		}
		return scale;
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawLine(transform.position,transform.position+transform.forward * m_speed);
		Gizmos.DrawLine(transform.position,m_leftBumper.position);
		Gizmos.DrawLine(transform.position,m_rightBumper.position);
	}
}