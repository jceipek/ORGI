using UnityEngine;
using System.Collections;

public class Dashboard : MonoBehaviour {
	private pulseSync m_pulseSyncComponent;

	// Use this for initialization
	void Start ()
	{
		m_pulseSyncComponent = GetComponent<pulseSync>();
	}
	
	void OnGUI ()
	{
		GUI.color = Color.black;
		GUI.Label(new Rect(10, 10, 100, 20), m_pulseSyncComponent.m_BPM.ToString());

	}

	// Update is called once per frame
	void Update () {
	
	}
}
