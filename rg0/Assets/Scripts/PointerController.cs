using UnityEngine;
using System.Collections;

public class PointerController : MonoBehaviour
{

	public float m_movementScale;
	public Vector3 m_positionOffset;

	private Camera m_camera;
	private Vector3 m_toolDirection;

	void OnEnable ()
	{
		m_camera = Camera.main;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnDrawGizmos ()
	{
		// display the ray of the pointer
		Vector3 origin = m_camera.transform.position;
		Gizmos.DrawRay(origin, m_toolDirection);
	}

	// When the LeapManager detects that the leap has moved,
	// it calls ToolMoved
	public void ToolMoved (Vector3 position, Vector3 direction)
	{
		// we don't want to worry about where it is in the z axis
		position.z = 0;
		m_toolDirection = direction;
		m_toolDirection.z = 1;
		m_toolDirection *= 100;
		transform.position = (position * m_movementScale) + m_positionOffset;
	}
}
