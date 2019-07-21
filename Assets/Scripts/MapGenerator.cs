using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    [Range(0, 10)]
    public int smoothingIterations;

    [Range(0, 100)]
    public int fillThreshold = 70;

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
        Map map = new Map(mapWidth, mapHeight);
        RandomFillMap(map);

        for (int i = 0; i < smoothingIterations; i++) {
            SmoothMap(map);
        }

        if (fillThreshold > 0) RemoveSmallRegions(map, 1, 0);

        List<List<Coord>> landRegions = GetRegions(map, 0);

        foreach (List<Coord> region in landRegions) {
            foreach (Coord coord in region) {
                if (map.ActiveNeighborCount(coord.X, coord.Y, 1) > 0) {
                    //map[coord.X, coord.Y] = 255;
                }
            }
        }


        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateTileMap(map);
    }

    void RandomFillMap(Map map) {
        if (useRandomSeed) seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                if (x > borderSize && x <= map.Width - borderSize && y > borderSize && y <= map.Height - borderSize) {
                    map[x, y] = (byte)(pseudoRandom.Next(0, 100) < randomFillPercent ? 0 : 1);
                } else map[x, y] = 1;
            }
        }
    }

    void SmoothMap(Map map) {
        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                byte activeNeighborCount = map.ActiveNeighborCount(x, y, 0);
                if (activeNeighborCount > 4) {
                    map[x, y] = 0;
                } else if (activeNeighborCount < 4) {
                    map[x, y] = 1;
                }
            }
        }
    }

    void RemoveSmallRegions(Map map, byte toFind, byte toChange) {
        List<List<Coord>> wallRegions = GetRegions(map, toFind);
        foreach (List<Coord> region in wallRegions) {
            if (region.Count < fillThreshold) {
                foreach (Coord coord in region) {
                    map[coord.X, coord.Y] = toChange;
                }
            }
        }
    }

    List<List<Coord>> GetRegions(Map map, byte toFind) {
        List<List<Coord>> regions = new List<List<Coord>>();
        Map mapFlags = new Map(map.Width, map.Height);

        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                if (mapFlags[x, y] == 0 && map[x, y] == toFind) {
                    List<Coord> region = GetRegionCoords(map, x, y);
                    regions.Add(region);
                    foreach (Coord coord in region) {
                        mapFlags[coord.X, coord.Y] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionCoords(Map map, int startX, int startY) {
        List<Coord> coords = new List<Coord>();
        Map mapFlags = new Map(map.Width, map.Height);
        int targetTileType = map[startX, startY];
        Queue<Coord> toCheck = new Queue<Coord>();
        toCheck.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (toCheck.Count > 0) {
            Coord coord = toCheck.Dequeue();
            coords.Add(coord);
            for (int x = coord.X - 1; x <= coord.X + 1; x++) {
                for (int y = coord.Y - 1; y <= coord.Y + 1; y++) {
                    if (map.IsInRange(x, y) && (coord.X == x || coord.Y == y)) {
                        if (mapFlags[x, y] == 0 && map[x, y] == targetTileType) {
                            mapFlags[x, y] = 1;
                            toCheck.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return coords;
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