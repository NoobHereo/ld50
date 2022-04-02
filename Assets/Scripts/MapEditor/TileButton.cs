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

    public void Init(XElement tileXML, string tileName)
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();

        btn.onClick.AddListener(OnClick);
        TileXML = tileXML;
        TileName = tileName;
        //if (AssetHandler.GetSpriteFromXML(TileXML) == null)
        //    Debug.LogError("sprite was null");

        img.sprite = AssetHandler.GetSpriteFromXML(TileXML);
    }

    private void OnClick()
    {
        // TODO: Implement something here
    }
}