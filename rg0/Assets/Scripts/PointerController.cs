using UnityEngine;
using System.Collections;
using Leap;

public class PointerController : MonoBehaviour
{

	public AttackAbility m_attackAbility;
	public BlockAbility m_blockAbility;
	public float m_movementScale;
	public Vector3 m_positionOffset;

	public bool m_cameraFlipped;
	private Camera m_camera;

	// When the LeapManager detects that the leap has moved,
	// it calls ToolMoved
	public void ToolMoved (Vector3 position)
	{
		// we don't want to worry about where it is in the z axis
		position.z = 0;
		if (m_cameraFlipped)
		{
			position.x *= -1;
		}
		transform.localPosition = (position * m_movementScale) + m_positionOffset;
	}

	public void Gesture (Gesture gesture)
	{
		if (gesture.Type == Leap.Gesture.GestureType.TYPECIRCLE)
		{
			Debug.Log("Gesture type circle arrived at pointercontroller");
			m_attackAbility.SendAttack();
		}
		else if (gesture.Type == Leap.Gesture.GestureType.TYPESWIPE)
		{
			Debug.Log("Gesture type swipe arrived at pointercontroller");
			m_blockAbility.InitiateBlock();
		}
	}
}
