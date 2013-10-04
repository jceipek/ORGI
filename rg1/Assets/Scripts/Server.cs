using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour
{
	public static Server g;
	public NetworkMode m_networkMode;

	public bool m_testLocally = false;
	public string m_masterServerIp = "localhost";

	private NetworkView m_networkView;
	private static string GAME_NAME = "RG1";
	private GameObject m_enemyPrefab;

	public enum NetworkMode {Server, Client};

	private NetworkViewID m_avatarNetworkViewID;

	void OnEnable ()
	{
		g = this;
		m_networkView = GetComponent<NetworkView>();
		m_enemyPrefab = Resources.Load("Enemy") as GameObject;
	}

	void Awake ()
	{
		MasterServer.ipAddress = m_masterServerIp;
		MasterServer.port = 23466;
		MasterServer.ClearHostList();
		MasterServer.RequestHostList(GAME_NAME);
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start ()
	{
		switch (m_networkMode)
		{
			case NetworkMode.Server:
				Application.LoadLevel("controller");
				break;
			case NetworkMode.Client:
				Application.LoadLevel("player");
				break;
			default:
				break;
		}
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
	}

	public bool IsClient ()
	{
		return Server.g.m_networkMode == NetworkMode.Client;
	}

	public bool IsServer ()
	{
		return Server.g.m_networkMode == NetworkMode.Server;
	}

	public void ConnectAvatar (NetworkView view)
	{
		if (IsServer())
		{
			NetworkViewID networkViewID = Network.AllocateViewID();
			m_avatarNetworkViewID = networkViewID;
		}
		view.viewID = m_avatarNetworkViewID;
	}

	public void SpawnSound (Vector3 location)
	{
		//NetworkViewID networkViewID = Network.AllocateViewID();
		//m_networkView.RPC("RemoteCreatePlayer", RPCMode.AllBuffered, networkViewID, location);
		m_networkView.RPC("OtherRemoteCreatePlayer", RPCMode.Others, location);
	}

	private void MakeHostOrClient ()
	{
		bool serverExists = MasterServer.PollHostList().Length != 0;
		switch (m_networkMode)
		{
			case NetworkMode.Server:
				TryStartingHost(serverExists);
				break;
			case NetworkMode.Client:
				TryStartingClient(serverExists);
				break;
			default:
				break;
		}
	}

	private void TryStartingHost (bool serverExists)
	{
		if (!serverExists)
		{
			Debug.Log("Starting Host");
			Network.InitializeServer(1, 5000, !Network.HavePublicAddress());
			MasterServer.RegisterHost(GAME_NAME, "Game Instance");

			// if testing on one computer, you can create a test client here
			// CreateTestPlayer(1);
			if (m_testLocally)
			{
				//GameObject player = CreateTestPlayer(1);
			}
		}
		else
		{
			Debug.LogError("Existing Host! Can't Start!");
		}
	}

	private void TryStartingClient (bool serverExists)
	{
		if (serverExists)
		{
			Debug.Log("Starting Client");
			HostData[] hostData = MasterServer.PollHostList();
			Network.Connect(hostData[0]);
		}
		else
		{
			//Debug.LogError("No Existing Host! Can't Start!");
		}
	}

	[RPC]
	private void OtherRemoteCreatePlayer (Vector3 location)
	{
		NetworkViewID networkViewID = Network.AllocateViewID();
		m_networkView.RPC("RemoteCreatePlayer", RPCMode.AllBuffered, networkViewID, location);
	}

	[RPC]
	private void RemoteCreatePlayer (NetworkViewID networkViewID, Vector3 location)
	{
		Quaternion initialRotation = Quaternion.identity;

		GameObject enemy = Instantiate(m_enemyPrefab, location, initialRotation) as GameObject;

		// get the network view id from the enemy
		NetworkView networkView = enemy.GetComponent<NetworkView>();
		networkView.viewID = networkViewID;

		if (networkViewID.isMine)
		{
		}
	}
}
