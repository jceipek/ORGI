using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Pulse : MonoBehaviour {
	public bool m_useArduino;
	private SerialPort stream = new SerialPort("/dev/tty.usbmodem411", 115200);

	private string m_arduinoString;
	public int m_BPM;
	public int m_pulse;

	private AudioSource m_audioSource;
	public float m_beatDelay;

	void OnEnable ()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	void Start ()
	{
		StartCoroutine(Beat());
		if (m_useArduino) stream.Open(); // Opens the serial port
	}

	// Update is called once per frame
	void Update ()
	{
		if (m_useArduino)
		{
			m_arduinoString = stream.ReadLine(); // Reads the data from the arduino card
			m_BPM = int.Parse(m_arduinoString.Split(',')[0]);
			m_pulse = int.Parse(m_arduinoString.Split(',')[1]);
		}	
	}

	IEnumerator Beat ()
	{
		while (true)
		{
			while (!m_audioSource.isPlaying) {
				m_audioSource.Play();
			}
			yield return new WaitForSeconds(m_beatDelay);
			Debug.Log("HI");
		}
	}
}
