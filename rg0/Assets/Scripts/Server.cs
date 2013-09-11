using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour
{
	// Make sure the client and server players are in different locations
	// and facing each other, so they can see the spells flying.
	public Vector3 m_initialServerLocation;
	public Vector3 m_initialClientLocation;

	public Quaternion m_initialServerRotation;
	public Quaternion m_initialClientRotation;

	public Color[] m_playerColors;

	public LeapManager m_leapManager;

	void Awake ()
	{
		MasterServer.ClearHostList();
		MasterServer.RequestHostList("SpellGame");
	}

	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
		switch (msEvent)
		{
			case MasterServerEvent.HostListReceived:
				MakeHostOrClient();
				break;
			case MasterServerEvent.RegistrationFailedNoServer:
				Debug.Log("NO MASTER SERVER");
				break;
			case MasterServerEvent.RegistrationSucceeded:
				break;
			default:
				break;
		}
	}

	void MakeHostOrClient ()
	{
		// if there's no server, let's make one
		if (MasterServer.PollHostList().Length == 0)
		{
			Debug.Log("There's no host");
			Network.InitializeServer(1, 5000, true);
			MasterServer.RegisterHost("SpellGame", "Game Instance");

			GameObject player = createPlayer(m_initialServerLocation, m_initialServerRotation);
			VisualizationController visualizationController = player.GetComponent<VisualizationController>();
			visualizationController.InitializeColors(m_playerColors[0]);
		}

		// otherwise, let's connect to the first server in the list
		else
		{
			Debug.Log("There's a host");
			HostData[] hostData = MasterServer.PollHostList();
			Network.Connect(hostData[0]);
		}

	}

	GameObject createPlayer (Vector3 initialLocation, Quaternion initialRotation)
	{
		GameObject player = Resources.Load("Player") as GameObject;
		player = Network.Instantiate(player, initialLocation, initialRotation, 0) as GameObject;
		m_leapManager.ConnectPlayer(player);
		return player;
	}

	void OnConnectedToServer ()
	{
		Debug.Log("Connected to server");
		GameObject player = createPlayer(m_initialClientLocation, m_initialClientRotation);
		VisualizationController visualizationController = player.GetComponent<VisualizationController>();
		visualizationController.InitializeColors(m_playerColors[1]);
	}
}
