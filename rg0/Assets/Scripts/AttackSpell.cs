using UnityEngine;
using System.Collections;

public class AttackSpell : MonoBehaviour
{

	private Rigidbody m_rigidbody;

	void OnEnable ()
	{
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Fire (Vector3 velocity, float lifetime)
	{
		m_rigidbody.velocity = velocity;
		StartCoroutine(DestroyAfterTime(lifetime));
	}

	private IEnumerator DestroyAfterTime (float lifetime)
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}
}
