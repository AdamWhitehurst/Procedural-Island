using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    [Range(0, 10)]
    public int smoothingIterations;

    [Range(1, 5)]
    public int generationPadding = 0;
    public int borderSize = 5;
    byte[,] map;

    void Start() {
        GenerateMap();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GenerateMap();
        }
    }

    void GenerateMap() {
        map = new byte[width * 2, height * 2];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap();
        }

        byte[,] borderedMap = new byte[width + borderSize * 2, height + borderSize * 2];
        for (int x = 0; x < borderedMap.GetLength(0); x++) {
            for (int y = 0; y < borderedMap.GetLength(1); y++) {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                } else {
                    borderedMap[x, y] = 1;
                }
            }
        }


        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateTileMap(borderedMap);
    }

    void RandomFillMap() {
        if (useRandomSeed) seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x < generationPadding || x >= width - generationPadding || y < generationPadding || y >= height - generationPadding) {
                    map[x, y] = 1;
                } else {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? (byte)1 : (byte)0;
                }
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighborWallCount = GetNeighborWallCount(x, y);
                if (neighborWallCount > 5) {
                    map[x, y] = 1;
                } else if (neighborWallCount < 4) {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetNeighborWallCount(int centerX, int centerY) {
        int wallCount = 0;
        for (int x = centerX - 1; x <= centerX + 1; x++) {
            for (int y = centerY - 1; y <= centerY + 1; y++) {
                if (centerX != x || centerY != y) {
                    if (x >= 0 && x < width && y >= 0 && y < height) {
                        wallCount += map[x, y];
                    } else {
                        wallCount += 2;
                    }
                }
            }
        }
        return wallCount;
    }

    // void OnDrawGizmos() {
    //     if (map != null) {
    //         for (int x = 0; x < width; x++) {
    //             for (int y = 0; y < height; y++) {
    //                 Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                 Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0);
    //                 Gizmos.DrawCube(pos, Vector3.one);
    //             }
    //         }
    //     }
    // }
}