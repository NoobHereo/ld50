using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartLevel : MonoBehaviour
{
    public static StartLevel Instance;
    public Button StartButton, BackButton;
    public TextMeshProUGUI Title, BestTime, Completed;
    [SerializeField] private string Name;

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

        GameData data = GameDataManager.LoadData();
        foreach(var level in data.CompletedLevels)
        {
            if (level.name == Name)
            {
                BestTime.text = "Best time: " + level.timeSeconds + " seconds.";
                string completed = level.completed ? "Yes" : "No";
                Completed.text = "Completed: " + completed;
                return;
            }
        }

        BestTime.text = "Best time: No time yet!";
        Completed.text = "Completed: No";
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