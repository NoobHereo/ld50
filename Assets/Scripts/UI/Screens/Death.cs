using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    public static Death Instance;
    public Button QuitButton, MenuButton;

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

    private void OnQuitClick()
    {
        Application.Quit();
    }

    private void OnMenuClick()
    {
        Menu.Instance.Dispatch(true);
        Dispatch(false);
    }
}