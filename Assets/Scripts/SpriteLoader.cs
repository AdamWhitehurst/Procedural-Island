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

        this.tileSprites = Resources.LoadAll<Sprite>("Water");
    }

    public Material GetTileMaterial() {
        return tileMaterial;
    }

    public Vector2[] GetTileUVs(int configIndex) {
        Vector2[] uvs = tileSprites[configIndex].uv;
        Vector2[] sortedUVs = new Vector2[] { uvs[1], uvs[3], uvs[2], uvs[0] };
        return sortedUVs;
    }

}
