using UnityEngine;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine.U2D;

public static class AssetHandler
{
    public static bool Loaded = false;
    public static Dictionary<string, XElement> TileXMLs = new Dictionary<string, XElement>();
    public static Dictionary<string, XElement> ObjectXMLs = new Dictionary<string, XElement>();
    public static Dictionary<string, XElement> WorldXMLs = new Dictionary<string, XElement>();

    public static void ParseTiles(TextAsset tileXML)
    {
        TileXMLs = new Dictionary<string, XElement>();

        var tiles = XElement.Parse(tileXML.text);
        foreach(var tile in tiles.Elements("Tile"))
        {
            TileXMLs.Add(tile.Attribute("name").Value, tile);
        }

        if (!Loaded)
            Loaded = true;
    }

    public static void ParseObjects(TextAsset objXML)
    {
        ObjectXMLs = new Dictionary<string, XElement>();

        var objects = XElement.Parse(objXML.text);
        foreach(var obj in objects.Elements("Object"))
        {
            ObjectXMLs.Add(obj.Attribute("name").Value, obj);
        }

        if (!Loaded)
            Loaded = true;
    }

    public static Sprite GetSpriteFromXML(XElement xml)
    {
        if (xml.Element("SpriteAsset") != null & xml.Element("SpriteIndex") != null)
        {
            string asset = xml.Element("SpriteAsset").Value;
            string index = xml.Element("SpriteIndex").Value;
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>($"Sprites/{xml.Name.LocalName + "Atlas"}");
            Sprite sprite = atlas.GetSprite(asset + "_" + index);
            return sprite;
        }
        else
        {
            Debug.LogError("The given xml is missing either SpriteAsset or SpriteIndex if not both.");
            return null;
        }
    }

    public static void ParseWorlds(TextAsset worldsXML)
    {
        WorldXMLs = new Dictionary<string, XElement>();

        var worlds = XElement.Parse(worldsXML.text);
        foreach(var worldXml in worlds.Elements("World"))
        {
            string name = worldXml.Attribute("name").Value;
            string resource = worldXml.Element("resource").Value;
            WorldXMLs.Add(name, worldXml);
        }

        if (LevelSelection.Instance != null)
            LevelSelection.Instance.Init();
    }
}