using UnityEngine;
using System.Collections;

public class BatAI : MonoBehaviour {

	public Transform m_leftBumper;
	public Transform m_rightBumper;

	public Vector3 m_direction;
	public float m_speed;
	public float m_rotSpeed;
	public float m_rotationFactor;

	void Start ()
	{

	}

	void FixedUpdate ()
	{
		MoveAvoid();
		if (!AvoidRight())
		{
			AvoidLeft();
		}
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
