using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using System.Text;

public class MapEditor : MonoBehaviour
{
    public static MapEditor Instance;

    [SerializeField] public bool Drawing { get; private set; }
    [SerializeField] public bool Erasing { get; private set; }

    public SimpleTile CurrentTile;
    public Dictionary<Vector3Int, SimpleTile> AddedTiles = new Dictionary<Vector3Int, SimpleTile>();
    public Dictionary<Vector3Int, SimpleTile> AddedObjects = new Dictionary<Vector3Int, SimpleTile>();

    public Tilemap TileLayer, ObjectLayer;
    public GameObject TileSelectionPanel, TileButtonPrefab, WorldButtonsPanel;
    public TextMeshProUGUI CurrentXmlDrawText;
    public Button DrawEraseButton, ShareButton, TilesButton, ObjectsButton, MenuButton;

    private void Start()
    {
        Instance = this;
        DrawEraseButton.onClick.AddListener(OnDrawEraseClick);
        ShareButton.onClick.AddListener(ExportMap);
        TilesButton.onClick.AddListener(OnTilesClick);
        ObjectsButton.onClick.AddListener(OnObjectsClick);
        MenuButton.onClick.AddListener(OnMenuClick);

        Drawing = true;
        Erasing = false;

        TileLayer.size = new Vector3Int(300, 300, 0);
        ObjectLayer.size = new Vector3Int(300, 300, 0);

        TextAsset tileXML = Resources.Load<TextAsset>("XML/Tiles");
        AssetHandler.ParseTiles(tileXML);
        TextAsset objXML = Resources.Load<TextAsset>("XML/Objects");
        AssetHandler.ParseObjects(objXML);
        TextAsset worldXML = Resources.Load<TextAsset>("XML/Worlds");
        AssetHandler.ParseWorlds(worldXML);

        LoadTiles();
        AddWorldButtons();
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
                tileBtn.GetComponent<TileButton>().Init(tile.Value, tile.Key);
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
                tileBtn.GetComponent<TileButton>().Init(obj.Value, obj.Key);
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

    private void UpdateGridCell(Vector3 point, SimpleTile tile, bool convertPos = true)
    {
        Tilemap tilemap = tile.Type == TileType.Tile ? TileLayer : ObjectLayer;
        Vector3Int pos = convertPos ? tilemap.WorldToCell(point) : new Vector3Int((int)point.x, (int)point.y, 0);

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

    private void OnMenuClick()
    {
        SceneManager.LoadScene("SplashScreen");
    }

    public void LoadMapRequest(string resource)
    {
        ClearMap();

        TextAsset json = Resources.Load<TextAsset>($"Maps/{resource}");
        SimpleTileData[] tileData = JsonHelper.FromJson<SimpleTileData>(json.text);
        foreach (SimpleTileData data in tileData)
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
                UpdateGridCell(new Vector3Int(pos.x, pos.y, 0), newTile, false);
            }

            // Objects
            else if (data.type == TileType.Object)
            {
                SimpleTile newTile = (SimpleTile)ScriptableObject.CreateInstance(typeof(SimpleTile));
                newTile.Name = data.name;
                newTile.Type = data.type;
                if (AssetHandler.ObjectXMLs.ContainsKey(data.name))
                {
                    Sprite sprite = AssetHandler.GetSpriteFromXML(AssetHandler.ObjectXMLs[data.name]);
                    newTile.Sprite = sprite;
                }
                UpdateGridCell(new Vector3Int(pos.x, pos.y, 0), newTile, false);
            }
        }
    }

    private void AddWorldButtons()
    {
        if (AssetHandler.Loaded)
        {
            Debug.Log("hi");
            foreach (var world in AssetHandler.WorldXMLs)
            {
                Debug.Log("yo");
                GameObject worldButton = Instantiate(Resources.Load<GameObject>("Prefabs/WorldButton"), WorldButtonsPanel.transform);
                worldButton.GetComponent<WorldButton>().Init(world.Key, world.Value.Element("resource").Value);
            }
        }
        else
        {
            Debug.LogError("Assethandler was not ready when called");
        }
    }

    public void ClearMap()
    {
        TileLayer.ClearAllTiles();
        ObjectLayer.ClearAllTiles();

        AddedTiles = new Dictionary<Vector3Int, SimpleTile>();
        AddedObjects = new Dictionary<Vector3Int, SimpleTile>();
    }
}