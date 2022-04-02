using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 300f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
    }

    public void InitCamera()
    {
        Camera.main.GetComponent<GameCamera>().SetTarget(transform);
    }

}