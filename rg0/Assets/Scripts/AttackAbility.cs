using UnityEngine;
using System.Collections;

public class AttackAbility : MonoBehaviour
{
	private NetworkView m_networkView;
	private GameObject m_opponent;
	private bool m_autoAttack = false;
	private float m_velocity = 10.0f;
	private int m_playerIndex = 0;

	void OnEnable ()
	{
		m_networkView = GetComponent<NetworkView>();
	}

	public void EnableAutoAttack ()
	{
		m_autoAttack = true;
		StartCoroutine("TimedAttack");
	}

	public void DisableAutoAttack ()
	{
		m_autoAttack = false;
		StopCoroutine("TimedAttack");
	}

	// find the opponent of the current player
	// so we can determine the direction we need to shoot the spell
	public GameObject GetOpponent ()
	{
		if (!m_opponent)
		{
			if (gameObject.name == "ClientPlayer")
			{
				m_opponent = GameObject.Find("HostPlayer");
				m_playerIndex = 1;
			}
			else if (gameObject.name == "HostPlayer")
			{
				m_opponent = GameObject.Find("ClientPlayer");
				m_playerIndex = 0;
			}
		}
		return m_opponent;
	}

	IEnumerator TimedAttack ()
	{
		yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));
		Vector3 initialLocation = gameObject.GetComponentInChildren<PointerController>().transform.position;
		Vector3 finalLocation = GetOpponent().transform.position;

		NetworkViewID spellViewID = Network.AllocateViewID();
		m_networkView.RPC("SpawnAttackSpell", RPCMode.AllBuffered, spellViewID, initialLocation, finalLocation);
		m_networkView.RPC("ReceiveAttack", RPCMode.Others);

		StartCoroutine("TimedAttack");
	}

	// Send attack to other player
	public void SendAttack ()
	{
		Vector3 initialLocation = gameObject.GetComponentInChildren<PointerController>().transform.position;
		Vector3 finalLocation = GetOpponent().transform.position;

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
		Vector3 velocity = direction * m_velocity;

		float lifetime = distance/m_velocity;

		foreach (Renderer renderer in attackSpell.GetComponentsInChildren<Renderer>())
		{
			renderer.material.SetColor ("_TintColor", Server.g.m_playerColors[m_playerIndex]);
		}

		attackSpell.GetComponent<AttackSpell>().Fire(velocity, lifetime);
	}

}
