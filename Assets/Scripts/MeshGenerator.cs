using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshGenerator : MonoBehaviour {

    public List<Vector3> vertices;

    public List<int> triangles;

    public List<Vector2> uvs;

    private Mesh mesh;

    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();
    private int colCount;

    private MeshCollider col;

    private int vertexCount;
    private int squareCount;

    void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();
    }


    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        squareCount = 0;
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        Mesh colMesh = new Mesh();
        colMesh.vertices = colVertices.ToArray();
        colMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = colMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;
    }

    public void GenerateTileMap(byte[,] map) {
        int z = (int)transform.position.z;
        for (int px = 0; px < map.GetLength(0); px++) {
            for (int py = 0; py < map.GetLength(1); py++) {
                GenerateSquare(px, py, z, map[px, py]);
                if (map[px, py] != 1)
                    GenerateCollider(px, py, z);
            }
        }
        UpdateMesh();
    }

    void GenerateSquare(int x, int y, int z, byte uvIndex) {

        int sqrIdx = squareCount * 4;

        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x, y - 1, z));
        vertices.Add(new Vector3(x + 1, y - 1, z));

        AddTriangle(sqrIdx + 0, sqrIdx + 1, sqrIdx + 2);
        AddTriangle(sqrIdx + 3, sqrIdx + 2, sqrIdx + 1);

        AddUVs(SpriteLoader.instance.GetTileUVs(uvIndex));

        squareCount++;
    }

    void GenerateCollider(int x, int y, int z) {
        int colIdx = colCount * 4;

        colVertices.Add(new Vector3(x, y, z));
        colVertices.Add(new Vector3(x + 1, y, z));
        colVertices.Add(new Vector3(x, y - 1, z));
        colVertices.Add(new Vector3(x + 1, y - 1, z));

        colTriangles.Add((colIdx) + 0);
        colTriangles.Add((colIdx) + 1);
        colTriangles.Add((colIdx) + 2);
        colTriangles.Add((colIdx) + 3);
        colTriangles.Add((colIdx) + 2);
        colTriangles.Add((colIdx) + 1);

        colCount++;
    }

    void AddUVs(Vector2[] newUVs) {
        this.uvs.AddRange(newUVs);
    }

    void AddTriangle(int a, int b, int c) {
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
    }
}
