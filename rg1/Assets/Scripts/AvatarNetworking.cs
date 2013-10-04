using UnityEngine;
using System.Collections;

public class AvatarNetworking : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		if (Server.g) Server.g.ConnectAvatar(GetComponent<NetworkView>());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
