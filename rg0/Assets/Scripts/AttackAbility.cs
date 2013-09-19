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

		Vector3 initialLocation = gameObject.GetComponentInChildren<PointerController>().transform.position;
		Vector3 finalLocation = m_opponent.transform.position;

		NetworkViewID spellViewID = Network.AllocateViewID();
		m_networkView.RPC("SpawnAttackSpell", RPCMode.AllBuffered, spellViewID, initialLocation, finalLocation);
		m_networkView.RPC("ReceiveAttack", RPCMode.Others);
	}

	[RPC]
	private void SpawnAttackSpell (NetworkViewID networkViewID, Vector3 initialLocation, Vector3 finalLocation)
	{

		Quaternion initialRotation = Quaternion.identity;

		GameObject attackSpell = Resources.Load("AttackSpell") as GameObject;
		attackSpell = Instantiate(attackSpell, initialLocation, initialRotation) as GameObject;
		NetworkView networkView = attackSpell.GetComponent<NetworkView>();
		networkView.viewID = networkViewID;


		Vector3 posDelta = finalLocation - initialLocation;
		float distance = posDelta.magnitude;
		Vector3 direction = posDelta.normalized;
		Vector3 velocity = direction * 5.0f;

		float lifetime = distance/5.0f;

		attackSpell.GetComponent<AttackSpell>().Fire(velocity, lifetime);
	}

}
