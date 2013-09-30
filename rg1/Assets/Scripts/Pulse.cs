using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Pulse : MonoBehaviour {
	private SerialPort stream = new SerialPort("/dev/tty.usbmodem411", 115200);
	private string pulse_string;
	private int pulse;
	
	// Use this for initialization
	void Start ()
	{
		stream.Open(); // Opens the serial port
	}

	// Update is called once per frame
	void Update ()
	{
		pulse_string = stream.ReadLine(); // Reads the data from the arduino card
		pulse = int.Parse(pulse_string);
		Debug.Log(pulse);
	}
}
