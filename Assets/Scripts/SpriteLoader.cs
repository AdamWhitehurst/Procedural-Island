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

        this.tileSprites = Resources.LoadAll<Sprite>("");
    }

    public Material GetTileMaterial() {
        return tileMaterial;
    }

    public Vector2[] GetTileUVs(int configIndex) {
        int tileMapIndex = configIndex == 0 ? 0 : 46;
        // if (Global.Tile_Config_To_Index_Map.ContainsKey(configIndex)) {
        //     tileMapIndex = Global.Tile_Config_To_Index_Map[configIndex];
        // }
        Vector2[] uvs = tileSprites[tileMapIndex].uv;
        Vector2[] sortedUVs = new Vector2[] { uvs[0], uvs[2], uvs[3], uvs[1] };
        return sortedUVs;
    }

}
