using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 diff;
    private Vector3 resetPoint;

    private bool dragging = false;
    private float zoomMin = 1f;
    private float zoomMax = 25f;
    private float sensivity = 10f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        resetPoint = transform.localPosition;
    }

    private void Update()
    {
        GetMovementInput();
        GetZoomInput();
    }

    private void GetMovementInput()
    {
        if (Input.GetMouseButton(1))
        {
            diff = (cam.ScreenToWorldPoint(Input.mousePosition)) - transform.localPosition;
            if (!dragging)
            {
                dragging = true;
                origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            dragging = false;
        }

        if (dragging)
            transform.position = origin - diff;
        if (Input.GetKeyDown(KeyCode.R))
            transform.position = resetPoint;
    }

    private void GetZoomInput()
    {
        // <>
        if (cam.orthographicSize >= zoomMin && cam.orthographicSize <= zoomMax)
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;

        if (cam.orthographicSize < zoomMin)
            SetOrthographicSize(zoomMin);
        if (cam.orthographicSize > zoomMax)
            SetOrthographicSize(zoomMax);
    }

    private void SetOrthographicSize(float size)
    {
        cam.orthographicSize = size;
    }
}