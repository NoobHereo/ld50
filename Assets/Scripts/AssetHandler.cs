using UnityEngine;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine.U2D;

public static class AssetHandler
{
    public static bool Loaded = false;
    public static Dictionary<XElement, string> Tiles = new Dictionary<XElement, string>();

    public static void Init(TextAsset tileXML)
    {
        var tiles = XElement.Parse(tileXML.text);
        foreach(var tile in tiles.Elements("Tile"))
        {
            string name = tile.Attribute("name").Value;
            Tiles.Add(tile, name);
        }
        Loaded = true;
    }

    public static Sprite GetSpriteFromXML(XElement xml)
    {
        if (xml.Element("SpriteAsset") != null & xml.Element("SpriteIndex") != null)
        {
            string asset = xml.Element("SpriteAsset").Value;
            string index = xml.Element("SpriteIndex").Value;
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>($"Sprites/{xml.Name.LocalName + "Atlas"}");
            Sprite sprite = atlas.GetSprite(xml.Element("SpriteAsset").Value + "_" + xml.Element("SpriteIndex").Value);
            return sprite;
        }
        else
        {
            Debug.LogError("The given xml is missing either SpriteAsset or SpriteIndex if not both.");
            return null;
        }
    }
}