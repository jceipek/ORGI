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

	public LeapManager m_leapManager;

	// Use this for initialization
	void Start ()
	{
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
        GameObject player = Resources.Load("Player") as GameObject;
        Vector3 initialLocation;
        Quaternion initialRotation;

		// if there's no server, let's make one
		if (MasterServer.PollHostList().Length == 0)
		{
        	Debug.Log("There's no host");
			Network.InitializeServer(1, 5000, true);
			MasterServer.RegisterHost("SpellGame", "Game Instance");
			initialLocation = m_initialServerLocation;
			initialRotation = m_initialServerRotation;
        }

        // otherwise, let's connect to the first server in the list
        else
        {
        	Debug.Log("There's a host");
            HostData[] hostData = MasterServer.PollHostList();
        	Network.Connect(hostData[0].ip, hostData[0].port);
        	initialLocation = m_initialClientLocation;
			initialRotation = m_initialClientRotation;
        }
        player = Network.Instantiate(player, initialLocation, initialRotation, 0) as GameObject;
        m_leapManager.ConnectPlayer(player);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
