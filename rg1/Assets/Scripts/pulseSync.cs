using UnityEngine;
using System.Collections;

public class pulseSync : MonoBehaviour {

	private NetworkView m_networkView;
	public int m_BPM = 0;
	private Pulse m_pulseComponent;

	void OnEnable () {
		m_networkView = GetComponent<NetworkView>();

		if (Server.g && Server.g.IsClient()) {
			NetworkViewID viewID = Network.AllocateViewID();
			m_networkView.viewID = viewID;
			Debug.Log(m_networkView.viewID);

			m_networkView.RPC("syncNetworkViewID", RPCMode.Others, m_networkView.viewID);
			m_pulseComponent = GetComponent<Pulse>();
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Server.g && Server.g.IsClient()) {
			m_BPM = m_pulseComponent.m_BPM;
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.Serialize(ref m_BPM);
	}

	[RPC]
	void syncNetworkViewID(NetworkViewID viewID) {
		m_networkView.viewID = viewID;
	}
}
