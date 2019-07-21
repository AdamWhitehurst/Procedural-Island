using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshGenerator : MonoBehaviour {

    public List<Vector3> vertices;

    public List<int> triangles;

    public List<Vector2> uvs;

    private Mesh mesh;
    public GameObject collidersParent;

    public Dictionary<Vector2, GameObject> collidersDict;
    private int squareCount;

    void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    void ResetMesh() {

        squareCount = 0;
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
    }

    void ResetColliders() {
        if (collidersParent) {
            Destroy(collidersParent);
        }
        collidersParent = new GameObject("Colliders");
        collidersParent.transform.parent = this.transform;

        collidersDict = new Dictionary<Vector2, GameObject>();
    }
    public void GenerateTileMap(Map map, int width, int height) {
        ResetColliders();

        float z = transform.position.z;
        int sqrX = 0;
        for (int x = 0; x < width - 1; x++) {
            int sqrY = 0;
            for (int y = 0; y < height - 1; y++) {
                byte botLeft = map[x + 0, y + 0];
                byte botRight = map[x + 1, y + 0];
                byte topLeft = map[x + 0, y + 1];
                byte topRight = map[x + 1, y + 1];
                byte tileUVIndex = GetAutoTile(sqrX, sqrY, map);
                GenerateSquare(sqrX, sqrY, z, tileUVIndex);
                if (botLeft == 1) {
                    GenerateCollider(sqrX + 0.25f, sqrY + 0.25f, z);
                }
                if (botRight == 1) {
                    GenerateCollider(sqrX + 0.75f, sqrY + 0.25f, z);
                }
                if (topLeft == 1) {
                    GenerateCollider(sqrX + 0.25f, sqrY + 0.75f, z);
                }
                if (topRight == 1) {
                    GenerateCollider(sqrX + 0.75f, sqrY + 0.75f, z);
                }
                sqrY++;

            }
            sqrX++;
        }
        UpdateMesh();
        ResetMesh();
    }

    byte GetAutoTile(int x, int y, Map map) {
        byte tileUVIndex = 0;
        tileUVIndex = (byte)(tileUVIndex << 1 | map[x + 0, y + 0]);
        tileUVIndex = (byte)(tileUVIndex << 1 | map[x + 1, y + 0]);
        tileUVIndex = (byte)(tileUVIndex << 1 | map[x + 0, y + 1]);
        tileUVIndex = (byte)(tileUVIndex << 1 | map[x + 1, y + 1]);

        return tileUVIndex;
    }


    void GenerateSquare(int x, int y, float z, byte tileUVIndex) {

        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x, y + 1, z));
        vertices.Add(new Vector3(x + 1, y + 1, z));

        int sqrIdx = squareCount * 4;

        AddTriangle(sqrIdx + 0, sqrIdx + 1, sqrIdx + 2);
        AddTriangle(sqrIdx + 3, sqrIdx + 2, sqrIdx + 1);

        AddUVs(SpriteLoader.instance.GetTileUVs(tileUVIndex));

        squareCount++;
    }

    void GenerateCollider(float x, float y, float z) {
        Vector2 vec = new Vector2(x, y);
        GameObject colObj = new GameObject();
        colObj.transform.parent = collidersParent.transform;
        BoxCollider2D col = colObj.AddComponent<BoxCollider2D>();
        col.name = $"{vec.x}, {vec.y}";
        col.size = new Vector2(0.5f, 0.5f);
        col.offset = new Vector2(vec.x, vec.y);
        collidersDict[vec] = colObj;
    }

    void AddUVs(Vector2[] newUVs) {
        this.uvs.AddRange(newUVs);
    }

    void AddTriangle(int a, int b, int c) {
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
    }

    GameObject GetColliderAt(float x, float y) {
        Vector2 vec = new Vector2(x, y);
        if (collidersDict.ContainsKey(vec)) {
            return collidersDict[vec];
        } else {
            Debug.LogError($"No Colliders at {x}, {y}");
            return null;
        }
    }
}
