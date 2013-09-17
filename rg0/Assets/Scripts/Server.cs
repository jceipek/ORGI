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

	private NetworkView m_networkView;

	private GameObject[] m_players;
	private int m_playerIndex;

	void OnEnable ()
	{
		m_networkView = GetComponent<NetworkView>();
		m_players = new GameObject[2];
	}

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

			m_playerIndex = 0;
			NetworkViewID viewID = Network.AllocateViewID();
			m_networkView.RPC("CreatePlayer", RPCMode.AllBuffered, m_playerIndex, viewID);
		}

		// otherwise, let's connect to the first server in the list
		else
		{
			Debug.Log("There's a host");
			HostData[] hostData = MasterServer.PollHostList();
			Network.Connect(hostData[0]);
		}

	}

	[RPC]
	GameObject CreatePlayer (int playerIndex, NetworkViewID viewID)
	{
		// set the position based on the player index
		Vector3 initialLocation = playerIndex == 1 ? m_initialServerLocation : m_initialClientLocation;
		Quaternion initialRotation = playerIndex == 1 ? m_initialServerRotation : m_initialClientRotation;

		// create the player object
		GameObject player = Resources.Load("Player") as GameObject;
		player = Instantiate(player, initialLocation, initialRotation) as GameObject;

		// get the network view id from the player
		NetworkView networkView = player.GetComponent<NetworkView>(); 
		networkView.viewID = viewID;

		// if the player is mine, connect it to the leap controller
		if (viewID.isMine)
		{
			m_leapManager.ConnectPlayer(player);
		}

		// save a local reference to the player
		m_players[playerIndex] = player;

		// set the color and the name
		VisualizationController visualizationController = player.GetComponent<VisualizationController>();
		visualizationController.InitializeColors(m_playerColors[playerIndex]);
		player.name = playerIndex == 1 ? "HostPlayer" : "ClientPlayer";

		return player;
	}


	void OnConnectedToServer ()
	{
		Debug.Log("Connected to server");

		m_playerIndex = 1;
		NetworkViewID viewID = Network.AllocateViewID();
		m_networkView.RPC("CreatePlayer", RPCMode.AllBuffered, m_playerIndex, viewID);
	}
}
