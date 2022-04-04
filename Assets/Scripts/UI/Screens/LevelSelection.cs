using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public static LevelSelection Instance;
    public Button EditorButton, BackButton, TutorialButton;
    public Menu Menu;
    public TextMeshProUGUI Spd, Dmg, Dex, Gold;
    public GameObject ButtonsPanel;

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
}