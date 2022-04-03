using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button Playbutton, EditorButton, QuitButton;

    private void Start()
    {
        Playbutton.onClick.AddListener(OnPlayClick);
        EditorButton.onClick.AddListener(OnEditorClick);
        QuitButton.onClick.AddListener(OnQuitClick);
    }

    private void OnPlayClick()
    {
        World.Instance.LoadWorld("Tutorial");
        Dispatch(false);        
    }

    private void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    private void OnEditorClick()
    {
        SceneManager.LoadScene("MapEditor");
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }
}