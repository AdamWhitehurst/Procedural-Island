using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Name: PaperWorld.cs
///
/// Description: 
/// </summary>
[Serializable]
public class PaperWorld {
    public enum WorldSize { Small, Medium, Large, Custom }

    #region Fields

    /// <summary>
    /// Serialized Fields
    /// </summary>
    // Name of the current world
    string name;
    // Size of the world
    WorldSize worldSize;
    // Width & Height of the current world
    int width, height;
    // List of layers in the world
    List<PaperWorldLayer> layers;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the name of this world.
    /// </summary>
    public string GetName { get { return name; } }
    /// <summary>
    /// Gets the size of this world.
    /// </summary>
    public WorldSize GetWorldSize { get { return worldSize; } }
    /// <summary>
    /// Gets the width of this world in tiles.
    /// </summary>
    public int GetWidth { get { return width; } }
    /// <summary>
    /// Gets the height of this world in tiles.
    /// </summary>
    public int GetHeight { get { return height; } }
    /// <summary>
    /// Gets the number of layers in this map.
    /// </summary>
    public int GetLayerCount { get { return layers.Count; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for the PaperWorld class.
    /// </summary>
    public PaperWorld() { }

    /// <summary>
    /// Overloaded constructor for the PaperWorld class that uses a pre-defined World Size.
    /// </summary>
    /// <param name="name">Name of this world.</param>
    public PaperWorld(string name, WorldSize worldSize) {
        this.name = name;
        this.worldSize = worldSize;

        if (GetWorldSize == WorldSize.Custom)
            MeasureWorld(WorldSize.Small);
        else
            MeasureWorld(GetWorldSize);

        layers = new List<PaperWorldLayer>();
    }

    /// <summary>
    /// Overloaded constructor for the PaperWorld class that uses custom World Size.
    /// </summary>
    /// <param name="name">Name of this world.</param>
    /// <param name="width">Width of this world in tiles.</param>
    /// <param name="height">Height of this world in tiles.</param>
    public PaperWorld(string name, int width, int height) {
        this.name = name;
        worldSize = WorldSize.Custom;
        this.width = width;
        this.height = height;
        layers = new List<PaperWorldLayer>();
    }

    #endregion

    #region Public Methods

    #region Layers

    /// <summary>
    /// Adds a new layer to the map.
    /// </summary>
    public void AddLayer(bool autoTile) {
        layers.Add(new PaperWorldLayer(GetWidth, GetHeight, autoTile));
    }

    /// <summary>
    /// Removes the last layer from the map.
    /// </summary>
    public void RemoveLayer() {
        layers.RemoveAt(layers.Count - 1);
    }

    #endregion

    #region Tiles

    /// <summary>
    /// Gets a tile value by x, y coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the tile.</param>
    /// <param name="y">Y coordinate of the tile.</param>
    /// <param name="layerID">Layer ID the tile is located in.</param>
    public ushort GetTileValue(int x, int y, int layerID) {
        return layers[layerID].GetTileValue(GetTileID(x, y));
    }

    /// <summary>
    /// Gets a tile sub value by x, y coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the tile.</param>
    /// <param name="y">Y coordinate of the tile.</param>
    /// <param name="layerID">Layer ID the tile is located in.</param>
    public byte GetTileSubValue(int x, int y, int layerID) {
        return layers[layerID].GetTileSubValue(GetTileID(x, y));
    }

    /// <summary>
    /// Sets a tile by x, y coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the tile.</param>
    /// <param name="y">Y coordinate of the tile.</param>
    /// <param name="layerID">Layer ID the tile is located in.</param>
    /// <param name="value">Value to set to the tile.</param>
    /// <param name="subValue">Sub Value to set to the tile.</param>
    public void SetTile(int x, int y, int layerID, ushort value, byte subValue) {
        layers[layerID].SetTile(GetTileID(x, y), value, subValue);
    }

    #endregion

    #region Decorations

    /// <summary>
    /// Gets a decoration value by x, y coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the decoration.</param>
    /// <param name="y">Y coordinate of the decoration.</param>
    /// <param name="layerID">Layer ID the decoration is located in.</param>
    public ushort GetDecorationValue(int x, int y, int layerID) {
        return layers[layerID].GetDecorationValue(GetTileID(x, y));
    }

    /// <summary>
    /// Gets a decoration sub value by coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the decoration.</param>
    /// <param name="y">Y coordinate of the decoration.</param>
    /// <param name="layerID">Layer ID the decoration is located in.</param>
    public byte GetDecorationSubValue(int x, int y, int layerID) {
        return layers[layerID].GetDecorationSubValue(GetTileID(x, y));
    }

    /// <summary>
    /// Sets a decoration by x, y coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the decoration.</param>
    /// <param name="y">Y coordinate of the decoration.</param>
    /// <param name="layerID">Layer ID the decoration is located in.</param>
    /// <param name="value">Value to set to the decoration.</param>
    /// <param name="subValue">Sub Value to set to the decoration.</param>
    public void SetDecoration(int x, int y, int layerID, ushort value, byte subValue) {
        layers[layerID].SetDecoration(GetTileID(x, y), value, subValue);
    }

    #endregion

    #region Utility

    /// <summary>
    /// Gets a tile ID by x, y coordinate.
    /// </summary>
    /// <param name="x">X coordinate of the tile.</param>
    /// <param name="y">Y coordinate of the tile.</param>
    public int GetTileID(int x, int y) {
        return (y * width) + x;
    }

    /// <summary>
    /// Saves the current Paper World to a file.
    /// </summary>
    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + name + Global.World_Data_Extension);
        bf.Serialize(file, this);
        file.Close();
    }

    #endregion

    #endregion

    #region Private Methods

    #region Utility

    /// <summary>
    /// Sets tile width & height of the world based on world size.
    /// </summary>
    private void MeasureWorld(WorldSize worldSize) {
        switch (worldSize) {
            case WorldSize.Small: // Small
                width = Global.SM_World_Width;
                height = Global.SM_World_Height;
                break;
            case WorldSize.Medium: // Medium
                width = Global.MED_World_Width;
                height = Global.MED_World_Height;
                break;
            case WorldSize.Large: // Large
                width = Global.LG_World_Width;
                height = Global.LG_World_Height;
                break;
        }
    }

    #endregion

    #endregion
}
