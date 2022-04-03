using UnityEngine;

public class HeightRendering : MonoBehaviour
{
    private float yThreshold = 100f;

    private void Update()
    {
        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
            renderer.sortingOrder = (int)(renderer.transform.position.y * -yThreshold);
    }
}