using UnityEngine;
using System.Collections;

public class AttackAbility : MonoBehaviour {

	private NetworkView m_networkView;

	void OnEnable ()
	{
		m_networkView = GetComponent<NetworkView>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Send attack to other player
	public void SendAttack () {
		m_networkView.RPC("ReceiveAttack", RPCMode.Others);
	}

}
