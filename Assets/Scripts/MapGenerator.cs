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

    Byte[] map;

    [Range(1, 10)]
    public int borderSize = 1;

    void Start() {
        GenerateMap();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GenerateMap();
        }
    }

    void GenerateMap() {
        map = new Byte[width * height];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap();
        }


        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateTileMap(map, width, height);
    }

    void RandomFillMap() {
        if (useRandomSeed) seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int i = x + width * y;
                if (x > borderSize && x <= width - borderSize && y > borderSize && y <= height - borderSize) {
                    map[i] = (byte)(pseudoRandom.Next(0, 100) < randomFillPercent ? 0 : 1);
                } else map[i] = 1;
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int i = x + width * y;
                int neighborWallCount = GetNeighborWallCount(x, y);
                if (neighborWallCount > 4) {
                    map[i] = 1;
                } else if (neighborWallCount < 4) {
                    map[i] = 0;
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
                        wallCount += map[x + width * y];
                    } else {
                        wallCount += 1;
                    }
                }
            }
        }
        return wallCount;
    }

    // void OnDrawGizmos() {
    //     for (int x = 0; x < width; x++) {
    //         for (int y = 0; y < height; y++) {
    //             Gizmos.color = (map[x + width * y] == 1) ? Color.white : Color.black;
    //             Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one / 5);
    //         }
    //     }
    // }
}