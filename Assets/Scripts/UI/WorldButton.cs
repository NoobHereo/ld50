using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldButton : MonoBehaviour
{

    private Button btn;
    public TextMeshProUGUI worldName;
    private string resource;

    public void Init(string name, string resource)
    {
        btn = GetComponent<Button>();
        worldName.text = name;
        this.resource = resource;
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MapEditor.Instance.LoadMapRequest(resource);
    }

}