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

	public GameObject[] m_players;
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

	void OnConnectedToServer ()
	{
		Debug.Log("Connected to server");
		CreatePlayer(1);
	}

	private void MakeHostOrClient ()
	{
		// if there's no server, let's make one
		if (MasterServer.PollHostList().Length == 0)
		{
			Debug.Log("There's no host");
			Network.InitializeServer(1, 5000, true);
			MasterServer.RegisterHost("SpellGame", "Game Instance");

			CreatePlayer(0);
			// if testing on one computer, you can create a test client here
			// CreateTestPlayer(1);
		}

		// otherwise, let's connect to the first server in the list
		else
		{
			Debug.Log("There's a host");
			HostData[] hostData = MasterServer.PollHostList();
			Network.Connect(hostData[0]);
		}

	}

	private void CreatePlayer (int playerIndex)
	{
		// Assign a networkview id so we can sync the players and create a player
		m_playerIndex = playerIndex;
		NetworkViewID playerViewID = Network.AllocateViewID();
		NetworkViewID pointerViewID = Network.AllocateViewID();
		m_networkView.RPC("RemoteCreatePlayer", RPCMode.AllBuffered, m_playerIndex, playerViewID, pointerViewID);
	}

	private GameObject CreateTestPlayer (int playerIndex)
	{
		NetworkViewID playerViewID = Network.AllocateViewID();
		NetworkViewID pointerViewID = Network.AllocateViewID();

		// set the position based on the player index
		Vector3 initialLocation = playerIndex == 0 ? m_initialServerLocation : m_initialClientLocation;
		Quaternion initialRotation = playerIndex == 0 ? m_initialServerRotation : m_initialClientRotation;

		// create the player object
		GameObject player = Resources.Load("Player") as GameObject;
		player = Instantiate(player, initialLocation, initialRotation) as GameObject;

		// get the network view id from the player
		NetworkView playerNetworkView = player.GetComponent<NetworkView>();
		playerNetworkView.viewID = playerViewID;

		Wand playerWand = player.GetComponent<Wand>();
		PointerController pointerController = playerWand.m_pointerController;
		NetworkView pointerNetworkView = pointerController.gameObject.GetComponent<NetworkView>();
		pointerNetworkView.viewID = pointerViewID;

		// save a local reference to the player
		m_players[playerIndex] = player;

		// set the color and the name
		VisualizationController visualizationController = player.GetComponent<VisualizationController>();
		visualizationController.InitializeColors(m_playerColors[playerIndex]);
		player.name = playerIndex == 0 ? "HostPlayer" : "ClientPlayer";

		return player;
	}

	[RPC]
	private GameObject RemoteCreatePlayer (int playerIndex, NetworkViewID playerViewID, NetworkViewID pointerViewID)
	{
		// set the position based on the player index
		Vector3 initialLocation = playerIndex == 0 ? m_initialServerLocation : m_initialClientLocation;
		Quaternion initialRotation = playerIndex == 0 ? m_initialServerRotation : m_initialClientRotation;

		// create the player object
		GameObject player = Resources.Load("Player") as GameObject;
		player = Instantiate(player, initialLocation, initialRotation) as GameObject;

		// get the network view id from the player
		NetworkView playerNetworkView = player.GetComponent<NetworkView>();
		playerNetworkView.viewID = playerViewID;

		Wand playerWand = player.GetComponent<Wand>();
		PointerController pointerController = playerWand.m_pointerController;
		NetworkView pointerNetworkView = pointerController.gameObject.GetComponent<NetworkView>();
		pointerNetworkView.viewID = pointerViewID;

		// if the player is mine, connect it to the leap controller
		if (playerViewID.isMine)
		{
			m_leapManager.ConnectPlayer(player);
		}

		// save a local reference to the player
		m_players[playerIndex] = player;

		// set the color and the name
		VisualizationController visualizationController = player.GetComponent<VisualizationController>();
		visualizationController.InitializeColors(m_playerColors[playerIndex]);
		player.name = playerIndex == 0 ? "HostPlayer" : "ClientPlayer";

		return player;
	}
}
