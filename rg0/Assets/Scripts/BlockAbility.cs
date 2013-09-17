using UnityEngine;
using System.Collections;

public class BlockAbility : MonoBehaviour
{

	public float m_blockDurationSeconds = 0.5f;
	public GameObject m_shield;
	private bool m_blocking = false;

	public bool IsBlocking ()
	{
		return m_blocking;
	}

	public void InitiateBlock ()
	{
		m_blocking = true;
		UpdateShieldDisplay();
		StartCoroutine(AbortBlockInSeconds(m_blockDurationSeconds));
	}

	private IEnumerator AbortBlockInSeconds (float seconds)
	{
		yield return new WaitForSeconds(seconds);
		m_blocking = false;
		UpdateShieldDisplay();
	}

	private void UpdateShieldDisplay ()
	{
		if (m_shield)
		{
			m_shield.SetActive(m_blocking);
		}
	}
}