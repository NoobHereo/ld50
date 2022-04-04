using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int Damage = 25;
    public float Speed = 300f;
    public int Health = 100;
    public int Rage = 100;

    public PlayerSword Sword;

    private Rigidbody2D rb;
    private PlayerAnimator animator;
    private PlayerSpriteState lastDir = PlayerSpriteState.IdleRight;
    public Slider HealthBar, Ragebar;

    private void Start()
    {
        Instance = this;
        HealthBar = GameObject.Find("Healthbar").GetComponent<Slider>();
        Ragebar = GameObject.Find("RageBar").GetComponent<Slider>();
        HealthBar.maxValue = 100;
        HealthBar.value = Health;
        Ragebar.maxValue = 100;
        Ragebar.value = Rage;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
    }

    private float lastRageTick = 0;

    private void Update()
    {

        if (Time.time - lastRageTick > 0.3f)
        {
            Rage--;
            Ragebar.value = Rage;

            if (Rage <= 0)
                World.Instance.PlayerDeath();

            lastRageTick = Time.time;
        }

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

        if (Input.GetKeyDown(KeyCode.Space))
            Sword.AnimateSwing(lastDir);

        rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
    }

    public void InitCamera()
    {
        Camera.main.GetComponent<GameCamera>().SetTarget(transform);
    }

    public void TakeDamage(int dmg)
    { 
        Health -= dmg;
        HealthBar.value = Health;

        if (Health <= 0)
        {
            World.Instance.PlayerDeath();
        }
    }

    public void GainRage(int rage)
    {
        if (Rage >= 100)
            return;

        Rage += rage;
        Ragebar.value = Rage;
    }

}