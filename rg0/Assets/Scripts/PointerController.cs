using UnityEngine;
using System.Collections;

public class PointerController : MonoBehaviour
{

	public float m_movementScale;
	public Vector3 m_positionOffset;

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
	public void ToolMoved(Vector3 position)
	{
		// we don't want to worry about where it is in the z axis
		position.z = 0;
		transform.position = (position * m_movementScale) + m_positionOffset;
	}
}
