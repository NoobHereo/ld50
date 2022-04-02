using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public enum TileType
{
    Tile,
    Object
}

[Serializable]
public class SimpleTileData
{
    public string name;
    public int posX, posY;
    public TileType type;
}

public class SimpleTile : TileBase
{
    public string Name;
    public Sprite Sprite;
    public TileType Type;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = Sprite;
        tileData.flags = TileFlags.None;
    }
}