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

    private Map gizmoMap;

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


        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        Map tileMap = meshGen.GenerateMap(map);
        Map tileSheetMap = new Map(tileMap.Width, tileMap.Height);
        Map edgeMap = GenerateEdgeMap(tileMap, 0, 15);

        List<List<Coord>> landRegions = GetRegions(tileMap, 0);
        foreach (List<Coord> region in landRegions) {
            foreach (Coord coord in region) {
                byte tileByteAddress = edgeMap.ActiveNeighborByte(coord.X, coord.Y, 1);
                if (edgeMap.IsInRange(coord.X, coord.Y) && edgeMap.ActiveNeighborCount(coord.X, coord.Y, 1) > 0) {
                    tileMap[coord.X, coord.Y] = tileByteAddress;
                    tileSheetMap[coord.X, coord.Y] = 1;
                }
            }
        }

        gizmoMap = edgeMap;
        meshGen.ApplyTileMap(tileSheetMap, tileMap);
        meshGen.ApplyMesh();
    }

    void RandomFillMap(Map map) {
        if (useRandomSeed) seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                if (x > borderSize && x <= map.Width - borderSize - 1 && y > borderSize && y <= map.Height - borderSize - 1) {
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

    Map GenerateEdgeMap(Map map, byte around, byte exclude) {

        Map edgeMap = new Map(map.Width, map.Height);
        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                byte tileByte = map[x, y];
                if (tileByte != around && tileByte != exclude) {
                    edgeMap[x, y] = 1;
                } else {
                    edgeMap[x, y] = 0;
                }
            }
        }
        return edgeMap;
    }

    void RemoveSmallRegions(Map map, byte toFill, byte fillWith) {
        List<List<Coord>> wallRegions = GetRegions(map, toFill);
        foreach (List<Coord> region in wallRegions) {
            if (region.Count < fillThreshold) {
                foreach (Coord coord in region) {
                    map[coord.X, coord.Y] = fillWith;
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

    void OnDrawGizmos() {
        if (gizmoMap == null) return;

        for (int x = 0; x < gizmoMap.Width; x++) {
            for (int y = 0; y < gizmoMap.Height; y++) {
                if (gizmoMap[x, y] == 0) Gizmos.color = Color.black;
                else if (gizmoMap[x, y] == 1) Gizmos.color = Color.white;
                else if (gizmoMap[x, y] == 2) Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one / 5);
            }
        }
    }
}