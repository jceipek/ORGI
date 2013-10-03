using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Pulse : MonoBehaviour {
	private SerialPort stream = new SerialPort("/dev/tty.usbmodem411", 115200);
	private string arduinoString;
	public int BPM;
	public int pulse;
	
	// Use this for initialization
	void Start ()
	{
		stream.Open(); // Opens the serial port
	}

	// Update is called once per frame
	void Update ()
	{
		arduinoString = stream.ReadLine(); // Reads the data from the arduino card
		BPM = int.Parse(arduinoString.Split(',')[0]);
		pulse = int.Parse(arduinoString.Split(',')[1]);
	}
}
