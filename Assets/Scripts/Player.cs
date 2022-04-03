using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 300f;
    public PlayerSword Sword;

    private Rigidbody2D rb;
    private PlayerAnimator animator;
    private PlayerSpriteState lastDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontalAbs = Mathf.Abs(horizontal);
        var verticalAbs = Mathf.Abs(vertical);

        if (horizontalAbs < 0.1f && verticalAbs < 0.1f)
        {
            animator.UpdateSpriteState(lastDir);
            rb.velocity = Vector3.zero;
            Sword.UpdateOrientation(lastDir);
        }

        if (horizontalAbs > verticalAbs)
        {
            animator.UpdateSpriteState(horizontal > 0.1f ? PlayerSpriteState.Right : PlayerSpriteState.Left);
            lastDir = horizontal > 0.1f ? PlayerSpriteState.IdleRight : PlayerSpriteState.IdleLeft;
            Sword.UpdateOrientation(horizontal > 0.1f ? PlayerSpriteState.Right : PlayerSpriteState.Left);
        }
        else if (verticalAbs > horizontalAbs)
        {
            animator.UpdateSpriteState(vertical > 0.1f ? PlayerSpriteState.Up : PlayerSpriteState.Down);
            lastDir = vertical > 0.1f ? PlayerSpriteState.IdleUp : PlayerSpriteState.IdleDown;
            Sword.UpdateOrientation(vertical > 0.1f ? PlayerSpriteState.Up : PlayerSpriteState.Down);
        }

        rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
    }

    public void InitCamera()
    {
        Camera.main.GetComponent<GameCamera>().SetTarget(transform);
    }

}