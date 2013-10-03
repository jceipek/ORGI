using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BatGenerator : MonoBehaviour {

	private MeshFilter m_meshFilter;
	private MeshRenderer m_meshRenderer;
	private int m_triangleCount = 10;
	private float m_size = 1.5f;

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
		for (int i = 0; i < m_triangleCount * 3; i++) {
			vertices[i] = Random.insideUnitSphere * m_size;
		}
		mesh.vertices = vertices;
	}
}