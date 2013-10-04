using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;

public class Pulse : MonoBehaviour {
	public bool m_useArduino;
	public string m_arduinoPort;
	private SerialPort m_stream;

	private string m_arduinoString;
	private int m_BPM;
	private int m_pulse;
	private int m_interBeatInterval;
	public float m_beatIntervalToBeatDelay = 1.0f/800.0f;

	private AudioSource m_audioSource;

	public float m_beatDelay = 0.5f;
	private float m_maxbeatDelay = 1.0f;


	void OnEnable ()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	void Start ()
	{

		DirectoryInfo dir = new DirectoryInfo("/dev/");
		FileInfo[] info = dir.GetFiles("tty.usbmodem*");
		foreach (FileInfo f in info)
		{
			m_arduinoPort = f.ToString();
		}

		m_stream = new SerialPort(m_arduinoPort, 115200);

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
			try {
				m_arduinoString = m_stream.ReadLine(); // Reads the data from the arduino card
				m_BPM = int.Parse(m_arduinoString.Split(',')[0]);
				m_pulse = int.Parse(m_arduinoString.Split(',')[1]);
				m_interBeatInterval = int.Parse(m_arduinoString.Split(',')[2]);
				if (m_interBeatInterval/1000.0f - m_audioSource.clip.length > 0.0f)
				{
					m_beatDelay = Mathf.Min(m_interBeatInterval/1000.0f - m_audioSource.clip.length, m_maxbeatDelay);
				}
			} catch {}

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
		}
	}

	void OnDestroy () {
		m_stream.Close();
	}
}
