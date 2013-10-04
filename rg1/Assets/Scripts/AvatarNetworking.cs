using UnityEngine;
using System.Collections;

public class AvatarNetworking : MonoBehaviour {

	public Vector3 m_playerPosition;
	public Quaternion m_playerRotation;

	void OnEnable ()
	{
		if (Server.g) Server.g.ConnectAvatar(GetComponent<NetworkView>());
	}
	
	void Update ()
	{
		if (Server.g && Server.g.IsClient())
		{
			m_playerPosition = transform.position;
			m_playerRotation = transform.rotation;
		}
	}
}
