using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour
{
	public static Server g;
	public NetworkMode m_networkMode;


	public bool m_connectionEstablished = false;

	public bool m_testLocally = false;
	public string m_masterServerIp = "localhost";

	private NetworkView m_networkView;
	private static string GAME_NAME = "RG1";
	private GameObject m_enemyPrefab;
	private GameObject m_soundPrefab;

	public enum NetworkMode {Server, Client};

	private NetworkViewID m_avatarNetworkViewID;

	void OnEnable ()
	{
		g = this;
		m_networkView = GetComponent<NetworkView>();
		m_enemyPrefab = Resources.Load("Enemy") as GameObject;
		m_soundPrefab = Resources.Load("ScarySound") as GameObject;
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
				Debug.Log("Host List Received");
				MakeHostOrClient();
				break;
			case MasterServerEvent.RegistrationFailedNoServer:
				Debug.Log("NO MASTER SERVER");
				break;
			case MasterServerEvent.RegistrationSucceeded:
				Debug.Log("RegistrationSucceeded");
				break;
			default:
				break;
		}
	}

	void OnServerInitialized() {
	    Debug.Log("Server initialized and ready");
	    m_connectionEstablished = true;
	}
	 
	void OnPlayerConnected(NetworkPlayer player) {
	    Debug.Log("Player " + " connected from " + player.ipAddress);
	}

	void OnConnectedToServer ()
	{
		Debug.Log("Connected to server");
		m_connectionEstablished = true;
	}

	public bool IsClient ()
	{
		return Server.g.m_networkMode == NetworkMode.Client;
	}

	public bool IsServer ()
	{
		return Server.g.m_networkMode == NetworkMode.Server;
	}

	public void SyncViewIds (NetworkView ownerView, string slaveTag)
	{
		NetworkViewID viewID = Network.AllocateViewID();
		ownerView.viewID = viewID;
		Debug.Log("Local ID:");
		Debug.Log(m_networkView.viewID);
		m_networkView.RPC("SyncNetworkViewID", RPCMode.Others, viewID, slaveTag);
		ownerView.stateSynchronization = NetworkStateSynchronization.Unreliable;
	}

	[RPC]
	public void SyncNetworkViewID(NetworkViewID viewID, string slaveTag) {
		Debug.Log("REMOTE ID:");
		Debug.Log(m_networkView.viewID);
		GameObject slaveObject = GameObject.FindWithTag(slaveTag);
		NetworkView networkView = slaveObject.GetComponent<NetworkView>();
		networkView.viewID = viewID;
		networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
	}

	public void SpawnSound (Vector3 location)
	{
		m_networkView.RPC("OtherRemoteCreateSound", RPCMode.Others, location);
	}

	public void SpawnEnemy (Vector3 location)
	{
		m_networkView.RPC("OtherRemoteCreateEnemy", RPCMode.Others, location);
	}

	public void StartGame()
	{
		Debug.Log("Game Started!");
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
	private void OtherRemoteCreateEnemy (Vector3 location)
	{
		NetworkViewID networkViewID = Network.AllocateViewID();
		m_networkView.RPC("RemoteCreateEnemy", RPCMode.AllBuffered, networkViewID, location);
	}

	[RPC]
	private void OtherRemoteCreateSound (Vector3 location)
	{
		NetworkViewID networkViewID = Network.AllocateViewID();
		m_networkView.RPC("RemoteCreateSound", RPCMode.AllBuffered, networkViewID, location);
	}

	[RPC]
	private void RemoteCreateEnemy (NetworkViewID networkViewID, Vector3 location)
	{
		RemoteCreateEntityHelper(m_enemyPrefab, networkViewID, location);
	}

	[RPC]
	private void RemoteCreateSound (NetworkViewID networkViewID, Vector3 location)
	{
		RemoteCreateEntityHelper(m_soundPrefab, networkViewID, location);
	}

	private void RemoteCreateEntityHelper(GameObject prefab, NetworkViewID networkViewID, Vector3 location)
	{
		Quaternion initialRotation = Quaternion.identity;

		GameObject instance = Instantiate(prefab, location, initialRotation) as GameObject;

		// get the network view id from the instance
		NetworkView networkView = instance.GetComponent<NetworkView>();
		networkView.viewID = networkViewID;

		if (networkViewID.isMine)
		{
		}
	}
}
