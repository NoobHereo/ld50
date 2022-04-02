using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using UnityEngine.U2D;

public class TileButton : MonoBehaviour
{
    private Button btn;
    private Image img;
    public XElement TileXML { get; private set; }
    public string TileName { get; private set; }
    public Sprite TileSprite { get; private set; }
    public TileType TileType { get; private set; }

    public void Init(XElement tileXML, string tileName)
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();

        btn.onClick.AddListener(OnClick);
        TileXML = tileXML;
        TileName = tileName;


        if (tileXML.Name.LocalName == "Tile")
            TileType = TileType.Tile;
        else if (tileXML.Name.LocalName == "Object")
            TileType = TileType.Object;

        TileSprite = AssetHandler.GetSpriteFromXML(TileXML);
        img.sprite = TileSprite;
    }

    private void OnClick()
    {
        MapEditor.Instance.SetDrawXML(TileName, TileSprite, TileType);
    }
}