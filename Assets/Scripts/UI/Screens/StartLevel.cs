using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartLevel : MonoBehaviour
{
    public static StartLevel Instance;
    public Button StartButton, BackButton;
    public TextMeshProUGUI Title;
    public string Name { get; private set; }

    private void Start()
    {
        Instance = this;
        StartButton.onClick.AddListener(OnStartClick);
        BackButton.onClick.AddListener(OnBackClick);
    }

    public void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    public void Init(string name)
    {
        Name = name;
        Title.text = Name;
    }

    private void OnStartClick()
    {
        Dispatch(false);
        World.Instance.LoadWorld(Name);
    }

    private void OnBackClick()
    {
        LevelSelection.Instance.Dispatch(true);
        Dispatch(false);
    }

}