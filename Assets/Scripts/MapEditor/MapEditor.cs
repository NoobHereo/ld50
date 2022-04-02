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

    public Tilemap TileLayer, ObjectLayer;
    public GameObject TileSelectionPanel, TileButtonPrefab;
    public TextMeshProUGUI CurrentXmlDrawText;
    public Button DrawEraseButton, ShareButton;

    private void Start()
    {
        Instance = this;
        DrawEraseButton.onClick.AddListener(OnDrawEraseClick);
        ShareButton.onClick.AddListener(ExportMap);

        Drawing = true;
        Erasing = false;

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
        TextAsset tileXML = Resources.Load<TextAsset>("XML/Tiles");
        AssetHandler.Init(tileXML);

        if (AssetHandler.Loaded)
        {
            foreach(var tile in AssetHandler.Tiles)
            {
                GameObject tileBtnPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/TileButton"), TileSelectionPanel.transform);
                tileBtnPrefab.GetComponent<TileButton>().Init(tile.Key, tile.Value);
            }
        }
    }

    public void SetDrawXML(string name, Sprite sprite, TileType type)
    {
        if (!Erasing)
        {
            CurrentXmlDrawText.text = "Drawing with: " + name;
            SimpleTile tile = (SimpleTile)ScriptableObject.CreateInstance(typeof(SimpleTile));
            tile.name = name;
            tile.Sprite = sprite;
            tile.Type = type;
            CurrentTile = tile;
        }
    }

    private void UpdateGridCell(Vector3 point, SimpleTile tile)
    {
        Tilemap tilemap = tile.Type == TileType.Tile ? TileLayer : ObjectLayer;
        Vector3Int pos = tilemap.WorldToCell(point);

        if (Erasing)
        {
            tilemap.SetTile(pos, null);
            AddedTiles.Remove(pos);
            return;
        }

        if (tilemap.GetTile(pos) == null)
        {
            tilemap.SetTile(pos, tile);
            AddedTiles.Add(pos, tile);
            return;
        }
        else if (tilemap.GetTile(pos) != tile)
        {
            Debug.Log("Replaced");
            tilemap.SetTile(pos, null);
            AddedTiles.Remove(pos);
            tilemap.SetTile(pos, tile);
            AddedTiles.Add(pos, tile);
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
        foreach(var tile in AddedTiles)
        {
            SimpleTileData data = new SimpleTileData();
            data.name = tile.Value.Name;
            data.posX = tile.Key.x;
            data.posY = tile.Key.y;
            data.type = tile.Value.Type;
            tileData.Add(data);
        }

        string json = JsonHelper.ToJson(tileData.ToArray(), true);
        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\mapFile.json", json);
    }
}