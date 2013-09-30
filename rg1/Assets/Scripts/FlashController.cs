using UnityEngine;
using System.Collections;

public class FlashController : MonoBehaviour
{

	public Light m_flashbeam;
	public AudioClip m_clickAudioClip;
	public bool m_autoBlink;

	public float m_fullIntensity = 0.14f;
	public float m_lightDuration = 0.2f; // seconds
	public float m_rejuvenateDuration = 0.8f; // seconds
	private bool m_ableToCast = true;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Fire1") || m_autoBlink)
		{
			if (m_ableToCast)
			{
				m_ableToCast = false;
				m_flashbeam.intensity = m_fullIntensity;
				StartCoroutine(RejuvenateLight(m_rejuvenateDuration));
				StartCoroutine(KillLight(m_lightDuration));
			}
			else if (!m_autoBlink)
			{
				AudioSource.PlayClipAtPoint(m_clickAudioClip, transform.position);
			}
		}
	}

    IEnumerator KillLight (float duration) {
        yield return new WaitForSeconds(duration);
        m_flashbeam.intensity = 0.0f;
    }

    IEnumerator RejuvenateLight (float duration) {
        yield return new WaitForSeconds(duration);
        m_ableToCast = true;
    }
}
