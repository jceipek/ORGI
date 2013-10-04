using UnityEngine;
using System.Collections;

public class pulseSync : MonoBehaviour {

	private NetworkView m_networkView;
	public int m_BPM = 0;
	private Pulse m_pulseComponent;

	void Start () {
		StartCoroutine(WaitForConnection());
	}


	void OnConnectedToServer ()
	{
		Debug.Log("CONNECTED!");

	}

	IEnumerator WaitForConnection ()
	{
		while (Server.g && !Server.g.m_connectionEstablished)
		{
			yield return new WaitForSeconds(0.1f);
		}
		Debug.Log("CONNECTION ESTABLISHED!");
		if (Server.g && Server.g.IsClient()) {

			m_networkView = GetComponent<NetworkView>();
			Server.g.SyncViewIds (m_networkView, "HeartBeat");
			m_pulseComponent = GetComponent<Pulse>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_pulseComponent) {
			m_BPM = m_pulseComponent.m_BPM;
			m_networkView.RPC("SyncBPM", RPCMode.Others, m_BPM);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		Debug.Log("SENDING");			Debug.Log("SENDING");
	 	stream.Serialize(ref m_BPM);
	}

	[RPC]
	public void SyncBPM(int BPM) {
		m_BPM = BPM;
	}
}
