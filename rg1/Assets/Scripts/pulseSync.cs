using UnityEngine;
using System.Collections;

public class pulseSync : MonoBehaviour {

	private NetworkView m_networkView;
	public int m_BPM = 0;
	private Pulse m_pulseComponent;

	void OnConnectedToServer ()
	{
		if (Server.g && Server.g.IsClient()) {

			m_networkView = GetComponent<NetworkView>();
			Server.g.SyncViewIds (m_networkView, "HeartBeat");
			m_pulseComponent = GetComponent<Pulse>();
		}
	}
	
	void Update ()
	{
		if (m_pulseComponent) {
			m_BPM = m_pulseComponent.m_BPM;
			m_networkView.RPC("SyncBPM", RPCMode.Others, m_BPM);
		}
	}

	[RPC]
	public void SyncBPM(int BPM) {
		m_BPM = BPM;
	}
}
