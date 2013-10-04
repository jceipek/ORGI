using UnityEngine;
using System.Collections;

public class pulseSync : MonoBehaviour {

	private NetworkView m_networkView;
	public int m_BPM = 0;
	private Pulse m_pulseComponent;

	void Start () {
		StartCoroutine(WaitForConnection());
	}

	IEnumerator WaitForConnection ()
	{
		while (Server.g && !Server.g.m_connectionEstablished)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (Server.g && Server.g.IsClient()) {
			NetworkViewID viewID = Network.AllocateViewID();

			m_networkView = GetComponent<NetworkView>();
			m_networkView.RPC("syncNetworkViewID", RPCMode.AllBuffered, viewID);
			m_pulseComponent = GetComponent<Pulse>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_pulseComponent) {
			m_BPM = m_pulseComponent.m_BPM;
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.Serialize(ref m_BPM);
	}

	[RPC]
	public void syncNetworkViewID(NetworkViewID viewID) {
		m_networkView = GetComponent<NetworkView>();
		m_networkView.viewID = viewID;
	}
}
