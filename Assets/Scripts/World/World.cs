using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Xml.Linq;
using cakeslice;
using TMPro;

public class World : MonoBehaviour
{
    public static World Instance;
    public string CurrentMap = "None";
    private string currentRes = "Hall";
    public Dictionary<Vector3Int, SimpleTile> Tiles = new Dictionary<Vector3Int, SimpleTile>();
    public Dictionary<Vector3Int, GameObject> Objects = new Dictionary<Vector3Int, GameObject>();
    public Tilemap Tilemap;
    public TextMeshProUGUI EnemiesLeft, Timer;
    private int enemies = 0;
    private int goldGain = 0;
    private int time = 0;
    private float lastTimeTick = 0;
    private bool count = false;

    private void Start()
    {
        Instance = this;
        Application.targetFrameRate = 144;

        TextAsset tileXML = Resources.Load<TextAsset>("XML/Tiles");
        AssetHandler.ParseTiles(tileXML);
        TextAsset objXML = Resources.Load<TextAsset>("XML/Objects");
        AssetHandler.ParseObjects(objXML);
        AssetHandler.ParseWorlds(Resources.Load<TextAsset>("XML/Worlds"));

        Timer.text = "Time: " + time.ToString();
    }

    private void Update()
    {
        if (Time.time - lastTimeTick > 1f && count)
        {
            time++;
            Timer.text = "Time: " + time.ToString();
            lastTimeTick = Time.time;
        }
    }

    public void LoadWorld(string world, bool custom = false, string json = "")
    {
        if (!custom)
        {
            foreach (var worldName in AssetHandler.WorldXMLs)
            {
                if (worldName.Key == world && worldName.Value.Element("resource") != null)
                {
                    CurrentMap = world;
                    RenderMap(worldName.Value.Element("resource").Value);
                }
            }
        }
        else if (custom)
        {
            CustomRender(json);
        }
    }

    public void RenderMap(string resource)
    {
        Tiles = new Dictionary<Vector3Int, SimpleTile>();
        Objects = new Dictionary<Vector3Int, GameObject>();
        currentRes = resource;

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
                GenerateObject(data, newPos);
            }
        }

        EnemiesLeft.text = "Enemies left: " + enemies;
        EnemiesLeft.gameObject.SetActive(true);
        InstantiatePlayer();
    }

    public void CustomRender(string json)
    {
        SimpleTileData[] tileData = JsonHelper.FromJson<SimpleTileData>(json);
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
                DrawTile(new Vector3Int(pos.x, pos.y, 0), newTile);
            }

            // Objects
            else if (data.type == TileType.Object)
            {
                GenerateObject(data, newPos);
            }
        }

        EnemiesLeft.text = "Enemies left: " + enemies;
        EnemiesLeft.gameObject.SetActive(true);
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
        count = true;
    }

    private void GenerateObject(SimpleTileData data, Vector3Int newPos)
    {
        if (data.name == "Tutorial WASD hud")
        {
            GameObject HUD = Instantiate(Resources.Load<GameObject>("Prefabs/ControlsHUD"));
            HUD.transform.position = new Vector3(data.posX, data.posY, 0);
        }
        else if (data.name == "Tutorial BARRELS hud")
        {
            GameObject HUD = Instantiate(Resources.Load<GameObject>("Prefabs/BarrelsHUD"));
            HUD.transform.position = new Vector3(data.posX, data.posY, 0);
        }
        else if (data.name == "Tutorial Enemy hud")
        {
            GameObject HUD = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyHUD"));
            HUD.transform.position = new Vector3(data.posX, data.posY, 0);
        }

        GameObject obj = new GameObject(data.name);
        obj.AddComponent<SpriteRenderer>();
        var renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sortingLayerID = SortingLayer.NameToID("Objects");

        obj.AddComponent<Rigidbody2D>();
        var rb = obj.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        XElement xml = null;

        if (AssetHandler.ObjectXMLs.ContainsKey(data.name))
        {
            xml = AssetHandler.ObjectXMLs[data.name];
            bool hidden = xml.Element("Hidden") != null ? true : false;
            Sprite sprite = hidden ? Resources.Load<Sprite>($"Sprites/Invisible") : AssetHandler.GetSpriteFromXML(xml);
            renderer.sprite = sprite;
        }
        bool canMove = false;

        if (xml.Element("NoMove") == null)
            canMove = true;

        if (!canMove)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (xml.Element("NoSort") == null)
            obj.AddComponent<HeightRendering>();

        if (xml.Element("HasCollider") != null)
        {
            obj.AddComponent<BoxCollider2D>();
            BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(1f, 1f);
            collider.offset = new Vector2(0, 0);

            collider.isTrigger = xml.Element("IsTrigger") != null ? true : false;
        }

        if (xml.Element("Enemy") != null)
        {
            GameObject dmgTrigger = new GameObject("DmgTrigger");
            dmgTrigger.transform.parent = obj.transform;
            dmgTrigger.transform.position = Vector3.zero;
            dmgTrigger.tag = "Enemy";

            dmgTrigger.AddComponent<BoxCollider2D>();
            BoxCollider2D dmgCol = dmgTrigger.GetComponent<BoxCollider2D>();
            dmgCol.size = new Vector2(1f, 1f);
            dmgCol.offset = new Vector2(0, 0.5f);
            dmgCol.isTrigger = true;

            int hp = 100;
            if (xml.Element("Health") != null)
                hp = int.Parse(xml.Element("Health").Value);

            int damage = 0;
            if (xml.Element("Damage") != null)
                damage = int.Parse(xml.Element("Damage").Value);

            dmgTrigger.AddComponent<EnemyDamageTrigger>();
            dmgTrigger.GetComponent<EnemyDamageTrigger>().Init(obj, hp, canMove, damage);
            enemies++;
        }

        obj.transform.position = newPos;
        
        if (xml.Element("NoOutline") == null)
            obj.AddComponent<Outline>();

        Objects.Add(newPos, obj);
    }

    public void EnemyDeath()
    {
        enemies--;
        EnemiesLeft.text = "Enemies left: " + enemies;
        if (enemies <= 0)
        {
            GainGold(UnityEngine.Random.Range(1, 5));
            CompleteLevel();
        }
    }

    private void GainGold(int gold)
    {
        goldGain += gold;
    }

    public void CompleteLevel()
    {
        count = false;
        LevelData lData = new LevelData();
        lData.name = CurrentMap;
        lData.resource = currentRes;
        lData.timeSeconds = time;
        lData.completed = true;

        GameData data = new GameData();
        data.gold = goldGain;
        data.CompletedLevels.Add(lData);
        GameDataManager.SaveData(data);
        goldGain = 0;

        Tilemap.ClearAllTiles();
        foreach(var obj in Objects)
        {
            Destroy(obj.Value);
        }

        if (CurrentMap == "Tutorial")
        {
            GameObject[] tutObjs = GameObject.FindGameObjectsWithTag("Tutorial");
            for (int i = 0; i < tutObjs.Length; i++)
                Destroy(tutObjs[i].gameObject);
        }

        Tiles = new Dictionary<Vector3Int, SimpleTile>();
        Objects = new Dictionary<Vector3Int, GameObject>();

        GameCamera.Instance.RemoveTarget();
        Destroy(Player.Instance.gameObject);
        LevelCompleted.Instance.Dispatch(true);
        LevelCompleted.Instance.Init(time);
        time = 0;
    }

    public void PlayerDeath()
    {
        count = false;
        time = 0;
        goldGain = 0;

        Tilemap.ClearAllTiles();
        foreach (var obj in Objects)
        {
            Destroy(obj.Value);
        }

        if (CurrentMap == "Tutorial")
        {
            GameObject[] tutObjs = GameObject.FindGameObjectsWithTag("Tutorial");
            for (int i = 0; i < tutObjs.Length; i++)
                Destroy(tutObjs[i].gameObject);
        }

        Tiles = new Dictionary<Vector3Int, SimpleTile>();
        Objects = new Dictionary<Vector3Int, GameObject>();

        GameCamera.Instance.RemoveTarget();
        Destroy(Player.Instance.gameObject);
        Death.Instance.Dispatch(true);
    }
}