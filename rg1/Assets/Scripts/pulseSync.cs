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
			// //m_networkView = GetComponent<NetworkView>();
			// NetworkViewID viewID = Network.AllocateViewID();
			// //m_networkView = (NetworkView)gameObject.AddComponent(typeof(NetworkView));
			// m_networkView.viewID = viewID;
			// m_networkView.RPC("syncNetworkViewID", RPCMode.Others, viewID);
			// m_pulseComponent = GetComponent<Pulse>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_pulseComponent) {
			m_BPM = m_pulseComponent.m_BPM;
			Debug.Log("SENDING");
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
	 	stream.Serialize(ref m_BPM);
	}

	// [RPC]
	// public void syncNetworkViewID(NetworkViewID viewID) {
	// 	//NetworkView networkView = GetComponent<NetworkView>();
	// 	//NetworkView networkView = (NetworkView)gameObject.AddComponent(typeof(NetworkView));
	// 	m_networkView = GetComponent<NetworkView>();
	// 	networkView.viewID = viewID;
	// }
}
