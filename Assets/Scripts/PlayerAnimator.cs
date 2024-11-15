using UnityEngine;

public enum PlayerSpriteState
{
    Right,
    Left,
    Up,
    Down,

    IdleRight,
    IdleLeft,
    IdleUp,
    IdleDown
}

public class PlayerAnimator : MonoBehaviour
{
    public Sprite[] RightSprites;
    public Sprite[] UpSprites;
    public Sprite[] DownSprites;
    private int rightLen;
    private int upLen;
    private int downLen;

    private SpriteRenderer sRenderer;
    private PlayerSpriteState state = PlayerSpriteState.IdleRight;

    private int counter;
    private float timer = 0;
    private float speed = 0.13f;

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        rightLen = RightSprites.Length;
        upLen = UpSprites.Length;
        downLen = DownSprites.Length;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < speed)
            return;

        timer = 0;
        UpdateSprite();
        counter++;
    }

    public void UpdateSpriteState(PlayerSpriteState state)
    {
        if (state == this.state)
            return;

        counter = 0;
        this.state = state;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        bool flipX = false;
        Sprite sprite = null;

        switch(state)
        {
            case PlayerSpriteState.Right:
                if (counter >= rightLen)
                    counter = 0;

                sprite = RightSprites[counter];
                flipX = false;
                break;

            case PlayerSpriteState.Left:
                if (counter >= rightLen)
                    counter = 0;

                sprite = RightSprites[counter];
                flipX = true;
                break;

            case PlayerSpriteState.Up:
                if (counter >= upLen)
                    counter = 0;

                sprite = UpSprites[counter];
                flipX = false;
                break;

            case PlayerSpriteState.Down:
                if (counter >= downLen)
                    counter = 0;

                sprite = DownSprites[counter];
                flipX = false;
                break;


            case PlayerSpriteState.IdleRight:
                sprite = RightSprites[0];
                flipX = false;
                break;

            case PlayerSpriteState.IdleLeft:
                sprite = RightSprites[0];
                flipX = true;
                break;

            case PlayerSpriteState.IdleUp:
                sprite = UpSprites[0];
                flipX = false;
                break;

            case PlayerSpriteState.IdleDown:
                sprite = DownSprites[0];
                flipX = false;
                break;
        }

        sRenderer.sprite = sprite;
        sRenderer.flipX = flipX;
    }
}