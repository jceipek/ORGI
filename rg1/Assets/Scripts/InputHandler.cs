﻿using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
	public float m_projectionDistance;
	private Camera m_camera;
	//private GameObject m_enemyPrefab;

	void OnEnable ()
	{
		m_camera = Camera.main;
		//m_enemyPrefab = Resources.Load("Enemy") as GameObject;
	}

	void Update ()
	{
		if (Input.GetButtonDown("SpawnEnemy"))
		{
			Ray spawnRay = m_camera.ScreenPointToRay(Input.mousePosition);
			Vector3 spawnPoint =  spawnRay.origin + spawnRay.direction * m_projectionDistance;
			Server.g.SpawnEnemy(spawnPoint);
		}
		if (Input.GetButtonDown("SpawnSound"))
		{
			Ray spawnRay = m_camera.ScreenPointToRay(Input.mousePosition);
			Vector3 spawnPoint =  spawnRay.origin + spawnRay.direction * m_projectionDistance;
			Server.g.SpawnSound(spawnPoint);
		}
		if (Input.GetButtonDown("StartGame"))
		{
			Server.g.StartGame();
		}
	}

	void OnDrawGizmos ()
	{
		if (!m_camera)
		{
			m_camera = Camera.main;
		}
		Gizmos.DrawWireCube(m_camera.transform.position +
			                m_camera.transform.forward * m_projectionDistance,
			                (Vector3.one + m_camera.transform.forward) * m_projectionDistance);
	}

}
