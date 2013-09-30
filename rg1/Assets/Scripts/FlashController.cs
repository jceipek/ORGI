using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FlashController : MonoBehaviour
{

	public Light m_flashbeam;
	public AudioClip m_clickAudioClip;

	private AudioSource m_audioSource;
	private float m_fullIntensity = 0.14f;
	private float m_lightDuration = 0.2f; // seconds
	private float m_rejuvenateDuration = 0.8f; // seconds
	private bool m_ableToCast = true;

	void OnEnable ()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (m_ableToCast)
			{
				m_ableToCast = false;
				m_flashbeam.intensity = m_fullIntensity;
				StartCoroutine(RejuvenateLight(m_rejuvenateDuration));
				StartCoroutine(KillLight(m_lightDuration));
			}
			else
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
