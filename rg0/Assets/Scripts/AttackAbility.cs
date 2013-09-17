using UnityEngine;
using System.Collections;

public class AttackAbility : MonoBehaviour
{
	private NetworkView m_networkView;
	private GameObject m_opponent;

	void OnEnable ()
	{
		m_networkView = GetComponent<NetworkView>();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Send attack to other player
	public void SendAttack ()
	{
		// find the opponent of the current player
		// so we can determine the direction we need to shoot the spell
		if (!m_opponent)
		{
			if (gameObject.name == "ClientPlayer")
			{
				m_opponent = GameObject.Find("HostPlayer");
			}
			else if (gameObject.name == "HostPlayer")
			{
				m_opponent = GameObject.Find("ClientPlayer");
			}
		}

		GameObject attackSpell = Resources.Load("AttackSpell") as GameObject;
		Vector3 initialLocation = new Vector3(0, 0, 0);
		Quaternion initialRotation = Quaternion.identity;
		attackSpell = Network.Instantiate(attackSpell, initialLocation, initialRotation, 0) as GameObject;
		Vector3 direction = m_opponent.transform.position - gameObject.transform.position;
		attackSpell.GetComponent<AttackSpell>().Fire(direction);
		m_networkView.RPC("ReceiveAttack", RPCMode.Others);
	}

}
