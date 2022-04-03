using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button Playbutton, QuitButton;
    public LevelSelection LevelSelection;

    private void Start()
    {
        Playbutton.onClick.AddListener(OnPlayClick);        
        QuitButton.onClick.AddListener(OnQuitClick);
    }

    private void OnPlayClick()
    {
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