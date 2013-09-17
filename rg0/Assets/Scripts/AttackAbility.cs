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


		Vector3 initialLocation = gameObject.GetComponentInChildren<PointerController>().transform.position;
		Vector3 finalLocation = m_opponent.GetComponentInChildren<PointerController>().transform.position;

		Quaternion initialRotation = Quaternion.identity;
		
		attackSpell = Network.Instantiate(attackSpell, initialLocation, initialRotation, 0) as GameObject;
		
		Vector3 direction = finalLocation - initialLocation;

		attackSpell.GetComponent<AttackSpell>().Fire(direction);
		m_networkView.RPC("ReceiveAttack", RPCMode.Others);
	}

}
