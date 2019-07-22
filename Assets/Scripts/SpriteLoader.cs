using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {
    public static SpriteLoader instance;
    public Material tileMaterial;
    Sprite[] tileSprites;
    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }

        this.tileSprites = Resources.LoadAll<Sprite>("AutoTiles/Sand_On_Water/Sand_On_Water");
        Debug.Log("Loaded Sprites");
    }

    public Material GetTileMaterial() {
        return tileMaterial;
    }

    public Vector2[] GetTileUVs(int tileMapIndex, int byteAddress) {
        if (tileMapIndex == 1) {
            if (!Global.Inner_Sand_Edge_Map.ContainsKey(byteAddress)) {
                Debug.LogWarning($"{byteAddress}");
                byteAddress = 31;
            } else byteAddress = Global.Inner_Sand_Edge_Map[byteAddress];
        }
        //Debug.Log($"{tileMapIndex} - {byteAddress}");
        Vector2[] uvs = tileSprites[byteAddress].uv;
        Vector2[] sortedUVs = new Vector2[] { uvs[3], uvs[1], uvs[0], uvs[2] };
        return sortedUVs;
    }

}
