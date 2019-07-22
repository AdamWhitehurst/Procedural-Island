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

    public void ApplyUVs() {
        mesh.uv = uvs.ToArray();
    }

    public void ApplyMesh() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        ApplyUVs();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    public void ApplyTileMap(Map tileSheetMap, Map tileMap) {
        uvs.Clear();
        for (int x = 0; x < tileMap.Width; x++) {
            for (int y = 0; y < tileMap.Height; y++) {
                AddUVs(SpriteLoader.instance.GetTileUVs(tileSheetMap[x, y], tileMap[x, y]));
            }
        }
    }

    public void ResetMesh() {
        squareCount = 0;
        vertices.Clear();
        triangles.Clear();
    }

    public void ResetColliders() {
        if (collidersParent) {
            Destroy(collidersParent);
        }
        collidersParent = new GameObject("Colliders");
        collidersParent.transform.parent = this.transform;

        collidersDict = new Dictionary<Vector2, GameObject>();
    }
    public Map GenerateMap(Map map) {
        ResetColliders();
        ResetMesh();

        Map tileMap = new Map(map.Width - 1, map.Height - 1);

        float z = transform.position.z;
        int sqrX = 0;
        for (int x = 0; x < map.Width - 1; x++) {
            int sqrY = 0;
            for (int y = 0; y < map.Height - 1; y++) {

                tileMap[x, y] = GetAutoTileByteAddress(x, y, map);

                GenerateTileColliders(x, y, z, sqrX, sqrY, map);
                GenerateMeshSquare(sqrX, sqrY, z);
                sqrY++;
            }
            sqrX++;
        }
        return tileMap;
    }

    byte GetAutoTileByteAddress(int x, int y, Map map) {
        byte tileUVByteAddress = 0;
        // Make anything that isn't 1 a 0
        byte botLeft = (byte)(map[x + 0, y + 0] != 0 ? 1 : 0);
        byte botRight = (byte)(map[x + 1, y + 0] != 0 ? 1 : 0);
        byte topLeft = (byte)(map[x + 0, y + 1] != 0 ? 1 : 0);
        byte topRight = (byte)(map[x + 1, y + 1] != 0 ? 1 : 0);

        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | botRight);
        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | botLeft);
        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | topRight);
        tileUVByteAddress = (byte)(tileUVByteAddress << 1 | topLeft);

        return tileUVByteAddress;
    }


    void GenerateMeshSquare(int x, int y, float z) {

        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x, y + 1, z));
        vertices.Add(new Vector3(x + 1, y + 1, z));

        int sqrIdx = squareCount * 4;

        AddTriangle(sqrIdx + 0, sqrIdx + 1, sqrIdx + 2);
        AddTriangle(sqrIdx + 3, sqrIdx + 2, sqrIdx + 1);

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
