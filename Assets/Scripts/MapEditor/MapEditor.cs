using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using System;

public class MapEditor : MonoBehaviour
{
    public static MapEditor Instance;

    [SerializeField] public bool Drawing { get; private set; }
    [SerializeField] public bool Erasing { get; private set; }

    public SimpleTile CurrentTile;
    public Dictionary<Vector3Int, SimpleTile> AddedTiles = new Dictionary<Vector3Int, SimpleTile>();
    public Dictionary<Vector3Int, SimpleTile> AddedObjects = new Dictionary<Vector3Int, SimpleTile>();

    public Tilemap TileLayer, ObjectLayer;
    public GameObject TileSelectionPanel, TileButtonPrefab;
    public TextMeshProUGUI CurrentXmlDrawText;
    public Button DrawEraseButton, ShareButton, TilesButton, ObjectsButton;

    private void Start()
    {
        Instance = this;
        DrawEraseButton.onClick.AddListener(OnDrawEraseClick);
        ShareButton.onClick.AddListener(ExportMap);
        TilesButton.onClick.AddListener(OnTilesClick);
        ObjectsButton.onClick.AddListener(OnObjectsClick);

        Drawing = true;
        Erasing = false;

        TextAsset tileXML = Resources.Load<TextAsset>("XML/Tiles");
        AssetHandler.ParseTiles(tileXML);
        TextAsset objXML = Resources.Load<TextAsset>("XML/Objects");
        AssetHandler.ParseObjects(objXML);

        LoadTiles();
    }

    private void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButton("Fire1") && CurrentTile != null && !EventSystem.current.IsPointerOverGameObject())
            UpdateGridCell(point, CurrentTile);
    }

    public void LoadTiles()
    {
        if (AssetHandler.Loaded)
        {
            foreach(var tile in AssetHandler.TileXMLs)
            {
                GameObject tileBtn = Instantiate(Resources.Load<GameObject>("Prefabs/TileButton"), TileSelectionPanel.transform);
                tileBtn.GetComponent<TileButton>().Init(tile.Key, tile.Value);
            }
        }
    }

    public void LoadObjects()
    {
        if (AssetHandler.Loaded)
        {
            foreach(var obj in AssetHandler.ObjectXMLs)
            {
                GameObject tileBtn = Instantiate(Resources.Load<GameObject>("Prefabs/TileButton"), TileSelectionPanel.transform);
                tileBtn.GetComponent<TileButton>().Init(obj.Key, obj.Value);
            }
        }
    }

    public void SetDrawXML(string name, Sprite sprite, TileType type)
    {
        if (!Erasing)
        {
            CurrentXmlDrawText.text = "Drawing with: " + name;
            SimpleTile tile = (SimpleTile)ScriptableObject.CreateInstance(typeof(SimpleTile));
            tile.Name = name;
            tile.Sprite = sprite;
            tile.Type = type;
            CurrentTile = tile;
        }
    }

    private void UpdateGridCell(Vector3 point, SimpleTile tile)
    {
        Tilemap tilemap = tile.Type == TileType.Tile ? TileLayer : ObjectLayer;
        Vector3Int pos = tilemap.WorldToCell(point);

        // Expand these statements on your own risk! (it's not pretty)
        if (Erasing)
        {
            tilemap.SetTile(pos, null);
            
            if (tile.Type == TileType.Tile)
                AddedTiles.Remove(pos);
            else if (tile.Type == TileType.Object)
                AddedObjects.Remove(pos);

            return;
        }
        if (tilemap.GetTile(pos) == null)
        {
            tilemap.SetTile(pos, tile);

            if (tile.Type == TileType.Tile)
                AddedTiles.Add(pos, tile);
            else if (tile.Type == TileType.Object)
                AddedObjects.Add(pos, tile);

            return;
        }
        else if (tilemap.GetTile(pos) != tile)
        {
            tilemap.SetTile(pos, null);
            if (tile.Type == TileType.Tile)
                AddedTiles.Remove(pos);
            else if (tile.Type == TileType.Object)
                AddedObjects.Remove(pos);

            tilemap.SetTile(pos, tile);
            if (tile.Type == TileType.Tile)
                AddedTiles.Add(pos, tile);
            else if (tile.Type == TileType.Object)
                AddedObjects.Add(pos, tile);

            return;
        }
    }

    private void OnDrawEraseClick()
    {
        Drawing = !Drawing;
        Erasing = !Erasing;

        string btnText = Drawing ? "Erase" : "Draw";
        string currTileName = CurrentTile == null ? "Nothing" : CurrentTile.Name;
        string drwText = Drawing ? "Drawing with: " + currTileName : "Erasing";

        CurrentXmlDrawText.text = drwText;
        DrawEraseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = btnText;
    }

    private void ExportMap()
    {
        Debug.Log(AddedTiles.Count);
        List<SimpleTileData> tileData = new List<SimpleTileData>();
        foreach (var tile in AddedTiles)
        {
            Debug.Log(tile.Value.Type.ToString());
            SimpleTileData data = new SimpleTileData();
            data.name = tile.Value.Name;
            data.posX = tile.Key.x;
            data.posY = tile.Key.y;
            data.type = tile.Value.Type;
            tileData.Add(data);
        }
        foreach (var obj in AddedObjects)
        {
            SimpleTileData data = new SimpleTileData();
            data.name = obj.Value.Name;
            data.posX = obj.Key.x;
            data.posY = obj.Key.y;
            data.type = obj.Value.Type;
            tileData.Add(data);
        }

        string json = JsonHelper.ToJson(tileData.ToArray(), true);
        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\mapFile.json", json);
    }

    private void OnTilesClick()
    {
        for(int i = 0; i < TileSelectionPanel.transform.childCount; i++)        
            Destroy(TileSelectionPanel.transform.GetChild(i).gameObject);

        LoadTiles();
    }

    private void OnObjectsClick()
    {
        for (int i = 0; i < TileSelectionPanel.transform.childCount; i++)
            Destroy(TileSelectionPanel.transform.GetChild(i).gameObject);

        LoadObjects();
    }
}