using UnityEngine;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine.U2D;

public static class AssetHandler
{
    public static bool Loaded = false;
    public static Dictionary<XElement, string> TileXMLs = new Dictionary<XElement, string>();
    public static Dictionary<XElement, string> ObjectXMLs = new Dictionary<XElement, string>();

    public static void ParseTiles(TextAsset tileXML)
    {
        var tiles = XElement.Parse(tileXML.text);
        foreach(var tile in tiles.Elements("Tile"))
        {
            TileXMLs.Add(tile, tile.Attribute("name").Value);
        }

        if (!Loaded)
            Loaded = true;
    }

    public static void ParseObjects(TextAsset objXML)
    {
        var objects = XElement.Parse(objXML.text);
        foreach(var obj in objects.Elements("Object"))
        {
            ObjectXMLs.Add(obj, obj.Attribute("name").Value);
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
}