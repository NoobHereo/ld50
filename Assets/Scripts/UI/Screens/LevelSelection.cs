using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button EditorButton, BackButton;
    public Menu Menu;

    public void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    private void Start()
    {
        EditorButton.onClick.AddListener(OnEditorClick);
        BackButton.onClick.AddListener(OnBackClick);
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


}