using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Tile,
    Object
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