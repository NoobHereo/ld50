using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleted : MonoBehaviour
{
    public static LevelCompleted Instance;
    public Button MenuButton, QuitButton;

    private void Start()
    {
        Instance = this;
        QuitButton.onClick.AddListener(OnQuitClick);
        MenuButton.onClick.AddListener(OnMenuClick);
    }

    public void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    private void OnMenuClick()
    {
        LevelSelection.Instance.Dispatch(true);
        Dispatch(false);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }

}