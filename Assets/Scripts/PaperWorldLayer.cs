using System;

/// <summary>
/// Name: PaperWorldLayer.cs
/// Description: TODO
/// </summary>
[Serializable]
public class PaperWorldLayer {
    #region Fields

    // Should this layer be auto tiled?
    bool autoTile;

    // TileSheet values for tiles & decorations
    ushort[] tileValues;
    ushort[] decorationValues;

    [NonSerialized]
    // Tile values for tiles
    byte[] tileSubValues;
    [NonSerialized]
    // Tile values for decorations
    byte[] decorationSubValues;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for the PaperWorldLayer class.
    /// </summary>
    public PaperWorldLayer() { }

    /// <summary>
    /// Overloaded constructor for the PaperWorldLayer class.
    /// </summary>
    /// <param name="worldWidth"></param>
    /// <param name="worldHeight"></param>
    /// <param name="autoTile"></param>
    public PaperWorldLayer(int worldWidth, int worldHeight, bool autoTile) {
        tileValues = new ushort[worldWidth * worldHeight];
        decorationValues = new ushort[worldWidth * worldHeight];
        tileSubValues = new byte[worldWidth * worldHeight];
        decorationSubValues = new byte[worldWidth * worldHeight];
        this.autoTile = autoTile;
    }

    #endregion

    #region Public Methods

    #region Tiles

    /// <summary>
    /// Gets a tile's value by tile ID.
    /// </summary>
    /// <param name="tileID">ID of the tile.</param>
    /// <returns></returns>
    public ushort GetTileValue(int tileID) {
        return tileValues[tileID];
    }

    /// <summary>
    /// Gets a tile's sub value by ID.
    /// </summary>
    /// <param name="tileID"></param>
    /// <returns></returns>
    public byte GetTileSubValue(int tileID) {
        return tileSubValues[tileID];
    }

    /// <summary>
    /// Sets a tile by tile ID, and also sets sub value.
    /// </summary>
    /// <param name="tileID">ID of the tile.</param>
    /// <param name="value">Value to set the tile to.</param>
    /// <param name="subValue">Sub Value to set the tile to.</param>
    public void SetTile(int tileID, ushort value, byte subValue) {
        tileValues[tileID] = value;
        tileSubValues[tileID] = subValue;
    }

    #endregion

    #region Decorations

    /// <summary>
    /// Gets a tile decoration's value by tile ID.
    /// </summary>
    /// <param name="tileID">ID of the decoration.</param>
    /// <returns></returns>
    public ushort GetDecorationValue(int tileID) {
        return decorationValues[tileID];
    }

    /// <summary>
    /// Gets a tile decoration's sub value by tile ID.
    /// </summary>
    /// <param name="tileID">ID of the decoration.</param>
    /// <returns></returns>
    public byte GetDecorationSubValue(int tileID) {
        return decorationSubValues[tileID];
    }

    /// <summary>
    /// Sets a tile decoration's sub value by tileID.
    /// </summary>
    /// <param name="tileID">ID of the decoration.</param>
    /// <param name="value">Value to set to the decoration.</param>
    /// <param name="subValue">Sub Value to set to the decoration.</param>
    public void SetDecoration(int tileID, ushort value, byte subValue) {
        decorationValues[tileID] = value;
        decorationSubValues[tileID] = subValue;
    }

    #endregion

    #region Auto Tiling

    /// <summary>
    /// Auto tiles the entire layer.
    /// </summary>
    public void AutoTileAll() {
        if (!autoTile)
            return;

        // TODO: Implement auto tiling algorithm
    }

    /// <summary>
    /// Auto tiles a single point.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AutoTilePoint(int x, int y) {
        if (!autoTile)
            return;

        // TODO: Implement auto tiling algorithm
    }

    #endregion

    #endregion
}
