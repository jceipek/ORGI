using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class BIOMDevice : MonoBehaviour
{
	// Imported functions from
	// BIOMPlugin.bundle (OSX)

	public static BIOMDevice g;

	void OnEnable ()
	{
		g = this;
	}

	[DllImport ("BIOMPlugin", EntryPoint="BIOM_Update")]
	private static extern int BIOM_Update();

	[DllImport ("BIOMDestroy")]
	private static extern bool BIOM_Destroy();

	void OnDestroy()
	{

	}
}