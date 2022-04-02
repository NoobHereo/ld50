using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Vector3 Offset = new Vector3(0, 0, 0);
    public Transform target { get ; private set; }
    private bool targetSet = false;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        if (target != null)
            targetSet = true;
        else
            targetSet = false;
    }

    private void LateUpdate()
    {
        if (targetSet)
        {
            transform.position = target.position + Offset;
        }
    }

}