using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public static World Instance;
    public string CurrentMap = "None";
    public Dictionary<Vector3Int, SimpleTile> Tiles = new Dictionary<Vector3Int, SimpleTile>();
    public Dictionary<Vector3Int, SimpleTile> Objects = new Dictionary<Vector3Int, SimpleTile>();
    public Tilemap Tilemap;

    private void Start()
    {
        Instance = this;

        TextAsset tileXML = Resources.Load<TextAsset>("XML/Tiles");
        AssetHandler.ParseTiles(tileXML);
        TextAsset objXML = Resources.Load<TextAsset>("XML/Objects");
        AssetHandler.ParseObjects(objXML);
        AssetHandler.ParseWorlds(Resources.Load<TextAsset>("XML/Worlds"));
    }

    public void LoadWorld(string world)
    {
        foreach(var worldName in AssetHandler.WorldXMLs)
        {
            if (worldName.Key == world && worldName.Value.Element("resource") != null)
            {
                CurrentMap = world;
                RenderMap(worldName.Value.Element("resource").Value);
            }
        }
    }

    public void RenderMap(string resource)
    {
        Tiles = new Dictionary<Vector3Int, SimpleTile>();
        Objects = new Dictionary<Vector3Int, SimpleTile>();

        TextAsset json = Resources.Load<TextAsset>($"Maps/{resource}");
        SimpleTileData[] tiles = JsonHelper.FromJson<SimpleTileData>(json.text);
        foreach(SimpleTileData tile in tiles)
        {
            Vector2Int pos = new Vector2Int(tile.posX, tile.posY);

            // Tiles
            if (tile.type == TileType.Tile)
            {
                SimpleTile newTile = (SimpleTile)ScriptableObject.CreateInstance(typeof(SimpleTile));
                newTile.Name = tile.name;
                newTile.Type = tile.type;
                if (AssetHandler.TileXMLs.ContainsKey(tile.name))
                {
                    Sprite sprite = AssetHandler.GetSpriteFromXML(AssetHandler.TileXMLs[tile.name]);
                    newTile.Sprite = sprite;
                }
                DrawTile(new Vector3Int(pos.x, pos.y, 0), newTile);
            }

            // Objects
            else if (tile.type == TileType.Object)
            {
                // This is supposed to be objects
            }
        }

        InstantiatePlayer();
    }

    public void DrawTile(Vector3Int pos, SimpleTile tile)
    {
        Tilemap.SetTile(pos, tile);      
    }

    public void InstantiatePlayer()
    {
        GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.GetComponent<Player>().InitCamera();
    }
}