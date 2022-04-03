using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public Button Playbutton, QuitButton;
    public LevelSelection LevelSelection;
    public TMP_InputField InputField;

    private void Start()
    {
        Playbutton.onClick.AddListener(OnPlayClick);        
        QuitButton.onClick.AddListener(OnQuitClick);

        if (!GameDataManager.DataExist())
            InputField.gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        if (!string.IsNullOrEmpty(InputField.text) && !GameDataManager.DataExist())
        {
            GameData data = new GameData()
            {
                username = InputField.text,
                speed = 300f,
                damage = 25,
                dexterity = 5f,
                gold = 0
            };

            GameDataManager.SaveData(data);
        }

        if (GameDataManager.DataExist())
            InputField.gameObject.SetActive(false);

        LevelSelection.Dispatch(true);
        Dispatch(false);        
    }

    public void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}