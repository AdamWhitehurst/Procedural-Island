using System;
/// <summary>
/// Name: Map.cs
///
/// Description: A class that store bytes in a one-dimensional
/// array with 2 dimensional coordinate accessors.
/// </summary>
public class Map {
    private Byte[] bytes;
    private int width;
    private int height;
    /// <summary>
    /// Default constructor for a map
    /// <param name="width">The width of the map</param>
    /// <param name="height">The height of the map</param>
    /// </summary>
    public Map(int width, int height) {
        this.width = width;
        this.height = height;

        bytes = new Byte[width * height];
    }

    public Byte this[int x, int y] {
        get => bytes[x + width * y];
        set => bytes[x + width * y] = value;
    }

    /// <summary>
    /// Gets the width of this map.
    /// </summary>
    public int Width { get { return width; } }
    /// <summary>
    /// Gets the height of this map.
    /// </summary>
    public int Height { get { return height; } }
}