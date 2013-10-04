using UnityEngine;
using System.Collections;

public class AvatarNetworking : MonoBehaviour {

	public Vector3 m_playerPosition;
	public Quaternion m_playerRotation;
	private NetworkView m_networkView;

	void OnConnectedToServer ()
	{
		if (Server.g && Server.g.IsClient()) {
			m_networkView = GetComponent<NetworkView>();
			Server.g.SyncViewIds (m_networkView, "Avatar");
		}
	}
	
	void Update ()
	{
		if (Server.g)
		{
			if (Server.g.IsServer())
			{
				transform.position = m_playerPosition;
				transform.rotation = m_playerRotation;	
			}
			else if (m_networkView && m_networkView.observed)
			{
				m_networkView.RPC("SyncTransform", RPCMode.Others, ((Transform)m_networkView.observed).rotation, ((Transform)m_networkView.observed).position);
			}
		}
	}

	[RPC]
	public void SyncTransform(Quaternion rotation, Vector3 position) {
		m_playerPosition = position;
		m_playerRotation = rotation;
	}
}