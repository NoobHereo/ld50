using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    public bool Drawing { get; private set; }
    public bool Erasing { get; private set; }

    public Tilemap TileLayer;
    public GameObject TileSelectionPanel, TileButtonPrefab;

    private void Start()
    {
        Drawing = true;
        Erasing = false;
        LoadTiles();
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
}