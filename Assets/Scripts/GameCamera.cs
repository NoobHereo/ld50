using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Vector3 Offset = new Vector3(0, 0, 0);
    public Transform target { get ; private set; }
    private bool targetSet = false;

    private void LateUpdate()
    {
        if (targetSet)
        {
            transform.position = target.position + Offset;
        }
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
        targetSet = true;
    }

}