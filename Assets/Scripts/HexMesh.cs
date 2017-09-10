using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh hexMesh;

	MeshCollider meshCollider;

    static List<Vector3> vertices = new List<Vector3>();
    static List<Color> colors = new List<Color>();
    static List<int> triangles = new List<int>();

    void Awake () {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
	}

	public void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	public void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

    public void Clear()
    {
        hexMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();
    }

    public void Apply()
    {
        hexMesh.SetVertices(vertices);
        hexMesh.SetColors(colors);
        hexMesh.SetTriangles(triangles, 0);
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }
}