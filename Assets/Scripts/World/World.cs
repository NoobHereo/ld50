using UnityEngine;

public class World : MonoBehaviour
{
    public static World Instance;

    private void Start()
    {
        Instance = this;
    }

    public void LoadWorld()
    {

    }
}