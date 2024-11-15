using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private Button btn;
    public TextMeshProUGUI LevelName;
    public string Name { get; private set; }
    public string Resource { get; private set; }

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void Init(string name, string resource)
    {
        Name = name;
        Resource = resource;
        LevelName.text = name;
    }

    private void OnClick()
    {
        LevelSelection.Instance.Dispatch(false);
        StartLevel.Instance.Dispatch(true);
        StartLevel.Instance.Init(Name);
    }


}