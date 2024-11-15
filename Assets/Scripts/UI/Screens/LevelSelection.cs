using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;

public class LevelSelection : MonoBehaviour
{
    public static LevelSelection Instance;
    public Button EditorButton, BackButton, TutorialButton, LoadButton, LoadConfirmButton;
    public Menu Menu;
    public TextMeshProUGUI Spd, Dmg, Dex, Gold;
    public GameObject ButtonsPanel, LoadPanel;

    public TMP_InputField LoadField;

    private void Awake()
    {
        Instance = this;
    }

    public void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    public void Init()
    {
        EditorButton.onClick.AddListener(OnEditorClick);
        BackButton.onClick.AddListener(OnBackClick);
        TutorialButton.onClick.AddListener(OnTutorial);
        LoadButton.onClick.AddListener(OnLoadClick);
        LoadConfirmButton.onClick.AddListener(OnLoadConfirmClick);

        foreach (var world in AssetHandler.WorldXMLs)
        {
            string name = world.Value.Attribute("name").Value;
            string resource = world.Value.Element("resource").Value;

            if (name != "Tutorial")
            {
                GameObject level = Instantiate(Resources.Load<GameObject>("Prefabs/LevelButton"), ButtonsPanel.transform);
                level.GetComponent<LevelButton>().Init(name, resource);
            }
        }

        if (GameDataManager.DataExist())
        {
            GameData data = GameDataManager.LoadData();
            Spd.text = "Speed: " + data.speed;
            Dmg.text = "Damage: " + data.damage;
            Dex.text = "Dexterity: " + data.dexterity;
            Gold.text = "Gold: " + data.gold;
        }
    }

    private void OnEditorClick()
    {
        SceneManager.LoadScene("MapEditor");
    }

    private void OnBackClick()
    {
        Menu.Dispatch(true);
        Dispatch(false);
    }

    private void OnTutorial()
    {
        Dispatch(false);
        World.Instance.LoadWorld("Tutorial");
    }

    private void OnLoadClick()
    {
        LoadPanel.gameObject.SetActive(true);
    }

    private void OnLoadConfirmClick()
    {
        string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string path = deskPath + @"\" + LoadField.text + ".json";
        string content;
        try
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                content = reader.ReadToEnd();
            }

            LoadPanel.gameObject.SetActive(false);
            Dispatch(false);
            World.Instance.LoadWorld("", true, content);
        }
        catch (Exception ex)
        {
            LoadPanel.SetActive(false);
            return;
        }

    }
}