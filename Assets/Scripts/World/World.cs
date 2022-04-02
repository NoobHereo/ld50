using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public static World Instance;
    public string CurrentMap = "None";
    public Dictionary<Vector3Int, SimpleTile> Tiles = new Dictionary<Vector3Int, SimpleTile>();
    public Dictionary<Vector3Int, GameObject> Objects = new Dictionary<Vector3Int, GameObject>();
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
        Objects = new Dictionary<Vector3Int, GameObject>();

        TextAsset json = Resources.Load<TextAsset>($"Maps/{resource}");
        SimpleTileData[] tileData = JsonHelper.FromJson<SimpleTileData>(json.text);
        foreach(SimpleTileData data in tileData)
        {
            Vector2Int pos = new Vector2Int(data.posX, data.posY);
            Vector3Int newPos = new Vector3Int(pos.x, pos.y, 0);

            // Tiles
            if (data.type == TileType.Tile)
            {
                SimpleTile newTile = (SimpleTile)ScriptableObject.CreateInstance(typeof(SimpleTile));
                newTile.Name = data.name;
                newTile.Type = data.type;
                if (AssetHandler.TileXMLs.ContainsKey(data.name))
                {
                    Sprite sprite = AssetHandler.GetSpriteFromXML(AssetHandler.TileXMLs[data.name]);
                    newTile.Sprite = sprite;
                }
                DrawTile(new Vector3Int(pos.x, pos.y, 0), newTile);
            }

            // Objects
            else if (data.type == TileType.Object)
            {
                GameObject obj = new GameObject(data.name);
                obj.AddComponent<SpriteRenderer>();
                if (AssetHandler.ObjectXMLs.ContainsKey(data.name))
                {
                    Sprite sprite = AssetHandler.GetSpriteFromXML(AssetHandler.ObjectXMLs[data.name]);
                    if (data.name == "Playerspawn")
                        obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Invisible");
                    else
                        obj.GetComponent<SpriteRenderer>().sprite = sprite;
                }
                obj.transform.position = newPos;
                Objects.Add(newPos, obj);
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
        GameObject spawn = GameObject.Find("Playerspawn");
        GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.GetComponent<Player>().InitCamera();
        player.transform.position = spawn.transform.position;
    }
}