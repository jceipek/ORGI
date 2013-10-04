using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Pulse : MonoBehaviour {
	public bool m_useArduino;
	private SerialPort m_stream = new SerialPort("/dev/tty.usbmodem621", 115200);

	private string m_arduinoString;
	private int m_BPM;
	private int m_pulse;
	private int m_interBeatInterval;
	public float m_beatIntervalToBeatDelay = 1.0f/800.0f;

	private AudioSource m_audioSource;

	public float m_beatDelay = 0.5f;

	void OnEnable ()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	void Start ()
	{
		StartCoroutine(Beat());
		if (m_useArduino) {
			m_stream.Open(); // Opens the serial port
			m_stream.ReadTimeout = 200;
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (m_useArduino)
		{
			m_arduinoString = m_stream.ReadLine(); // Reads the data from the arduino card
			m_BPM = int.Parse(m_arduinoString.Split(',')[0]);
			m_pulse = int.Parse(m_arduinoString.Split(',')[1]);
			m_interBeatInterval = int.Parse(m_arduinoString.Split(',')[2]);
			m_beatDelay = m_interBeatInterval * m_beatIntervalToBeatDelay;
		}	
	}

	IEnumerator Beat ()
	{
		while (true)
		{
			while (!m_audioSource.isPlaying) {
				m_audioSource.Play();
				yield return new WaitForSeconds(0.001f);
			}
			yield return new WaitForSeconds(m_beatDelay);
			Debug.Log("HI");
		}
	}

	void OnDestroy () {
		m_stream.Close();
	}
}
