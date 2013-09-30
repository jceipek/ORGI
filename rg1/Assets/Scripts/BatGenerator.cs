using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BatGenerator : MonoBehaviour {

	private MeshFilter m_meshFilter;
	private MeshRenderer m_meshRenderer;
	private int m_triangleCount = 10;
	private float m_size = 1.5f;
/*
	void Start ()
	{
		m_meshFilter = gameObject.GetComponent<MeshFilter>();

		Mesh mesh = new Mesh();
		mesh.name = "testMesh";
		mesh.Clear();
		Vector3[] vertices = new Vector3[4];
		vertices[0] = new Vector3(-10.0f, 0.0f, 0.0f);
		vertices[1] = new Vector3(10.0f, 0.0f, 0.0f);
		vertices[2] = new Vector3(-10.0f, 50.0f, 0.0f);
		vertices[3] = new Vector3( 10.0f, 50.0f, 0.0f);
		mesh.vertices = vertices;

		Vector2[] uv = new Vector2[4];
		uv[0] = new Vector2(0,0);
		uv[1] = new Vector2(1,0);
		uv[2] = new Vector2(0,1);
		uv[3] = new Vector2(1,1);
		mesh.uv = uv;

		int[] triangles = new int[6];
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 1;
		triangles[4] = 3;
		triangles[5] = 2;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
		m_meshFilter.mesh = mesh;
		mr.renderer.material.color = Color.white;
	}
*/


	void Start ()
	{
		m_meshFilter = gameObject.GetComponent<MeshFilter>();
		m_meshRenderer = gameObject.GetComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.name = "testMesh";
		mesh.Clear();

		Vector3[] vertices = new Vector3[m_triangleCount * 3];
		Vector2[] uv = new Vector2[m_triangleCount * 3];
		int[] triangles = new int[m_triangleCount * 3];
		for (int i = 0; i < m_triangleCount * 3; i++) {
			vertices[i] = Random.insideUnitSphere * m_size;
			uv[i] = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
			triangles[i] = i;
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		m_meshFilter.mesh = mesh;
		m_meshRenderer.renderer.material.color = Color.black;
	}

	void Update ()
	{
		Mesh mesh = m_meshFilter.mesh;
		Vector3[] vertices = mesh.vertices;
		//Vector3[] normals = mesh.normals;
		for (int i = 0; i < m_triangleCount * 3; i++) {
			vertices[i] = Random.insideUnitSphere * m_size;
		}
		mesh.vertices = vertices;
	}
}