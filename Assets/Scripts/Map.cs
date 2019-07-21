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

    public bool IsInRange(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public byte ActiveNeighborByte(int centerX, int centerY, byte activeType) {
        byte activeAddress = 0;
        for (int x = centerX - 1; x <= centerX + 1; x++) {
            for (int y = centerY - 1; y <= centerY + 1; y++) {
                if (centerX != x || centerY != y) {
                    if (IsInRange(x, y)) {
                        byte activeByte = (byte)(this[x, y] == activeType ? 1 : 0);
                        activeAddress = (byte)(activeAddress << 1 | activeByte);
                    } else {
                        activeAddress = (byte)(activeAddress << 1 | 0);
                    }
                }
            }
        }
        return activeAddress;
    }

    public byte ActiveNeighborCount(int centerX, int centerY, byte activeType) {
        byte activeAddress = this.ActiveNeighborByte(centerX, centerY, activeType);
        byte activeCount = 0;
        for (int i = 0; i < 8; i++) {
            activeCount += (byte)((activeAddress >> i) & 1);
        }
        return activeCount;
    }
}