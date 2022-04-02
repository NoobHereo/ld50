using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button Playbutton;

    private void Start()
    {
        Playbutton.onClick.AddListener(OnPlayClick);
    }

    private void OnPlayClick()
    {
        // Start the game
    }
}