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
    public void GenerateTileMap(Map map) {
        ResetColliders();

        float z = transform.position.z;
        int sqrX = 0;
        for (int x = 0; x < map.Width - 1; x++) {
            int sqrY = 0;
            for (int y = 0; y < map.Height - 1; y++) {
                byte tileUVIndex = GetByteAddress(sqrX, sqrY, map);
                GenerateSquare(sqrX, sqrY, z, tileUVIndex);
                GenerateTileColliders(x, y, z, sqrX, sqrY, map);
                sqrY++;
            }
            sqrX++;
        }
        UpdateMesh();
        ResetMesh();
    }

    byte GetByteAddress(int x, int y, Map map) {
        if (map[x, y] == 255) return 31;
        byte tileUVByteAddress = 0;
        // Make anything that isn't 1 a 0
        byte botLeft = (byte)(map[x + 0, y + 0] != 0 ? 1 : 0);
        byte botRight = (byte)(map[x + 1, y + 0] != 0 ? 1 : 0);
        byte topLeft = (byte)(map[x + 0, y + 1] != 0 ? 1 : 0);
        byte topRight = (byte)(map[x + 1, y + 1] != 0 ? 1 : 0);

        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | botLeft);
        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | botRight);
        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | topLeft);
        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | topRight);

        return tileUVByteAddress;
    }


    void GenerateSquare(int x, int y, float z, byte tileByteAddress) {

        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x, y + 1, z));
        vertices.Add(new Vector3(x + 1, y + 1, z));

        int sqrIdx = squareCount * 4;

        AddTriangle(sqrIdx + 0, sqrIdx + 1, sqrIdx + 2);
        AddTriangle(sqrIdx + 3, sqrIdx + 2, sqrIdx + 1);

        AddUVs(SpriteLoader.instance.GetTileUVs(tileByteAddress));

        squareCount++;
    }

    void GenerateTileColliders(int x, int y, float z, int sqrX, int sqrY, Map map) {
        byte botLeft = map[x + 0, y + 0];
        byte botRight = map[x + 1, y + 0];
        byte topLeft = map[x + 0, y + 1];
        byte topRight = map[x + 1, y + 1];
        if (botLeft != 0) {
            GenerateSubCollider(sqrX + 0.25f, sqrY + 0.25f, z);
        }
        if (botRight != 0) {
            GenerateSubCollider(sqrX + 0.75f, sqrY + 0.25f, z);
        }
        if (topLeft != 0) {
            GenerateSubCollider(sqrX + 0.25f, sqrY + 0.75f, z);
        }
        if (topRight != 0) {
            GenerateSubCollider(sqrX + 0.75f, sqrY + 0.75f, z);
        }
    }

    void GenerateSubCollider(float x, float y, float z) {
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
