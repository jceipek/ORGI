using UnityEngine;
using System.Collections;

public class AttackAbility : MonoBehaviour
{

	private NetworkView m_networkView;

	void OnEnable ()
	{
		m_networkView = GetComponent<NetworkView>();
	}

	// Send attack to other player
	public void SendAttack ()
	{
		GameObject attackSpell = Resources.Load("AttackSpell") as GameObject;
		Vector3 initialLocation = new Vector3(0, 0, 0);
		Quaternion initialRotation = Quaternion.identity;
		attackSpell = Network.Instantiate(attackSpell, initialLocation, initialRotation, 0) as GameObject;
		attackSpell.GetComponent<AttackSpell>().Fire(new Vector3(0, 10, 0));
		m_networkView.RPC("ReceiveAttack", RPCMode.Others);
	}

}