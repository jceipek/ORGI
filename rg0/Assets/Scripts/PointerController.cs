using UnityEngine;
using System.Collections;

public class PointerController : MonoBehaviour
{

	public float m_movementScale;
	public Vector3 m_positionOffset;

	private Camera m_camera;

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

	// When the LeapManager detects that the leap has moved,
	// it calls ToolMoved
	public void ToolMoved (Vector3 position)
	{
		// we don't want to worry about where it is in the z axis
		position.z = 0;
		transform.position = (position * m_movementScale) + m_positionOffset;
	}
}
