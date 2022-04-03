using UnityEngine;

public class PlayerSword : MonoBehaviour
{

    public void UpdateOrientation(PlayerSpriteState state)
    {
        switch(state)
        {
            case PlayerSpriteState.Right:
            case PlayerSpriteState.IdleRight:
                float x = transform.parent.position.x + 0.8f;
                float y = transform.parent.position.y + 0.18f;

                var pos = new Vector3(x, y, 0);
                var rot = Quaternion.Euler(0, 0, -45f);

                SetOrientation(pos, rot);
                break;


            case PlayerSpriteState.Left:
            case PlayerSpriteState.IdleLeft:
                float x2 = transform.parent.position.x + -0.8f;
                float y2 = transform.parent.position.y + 0.7f;

                var pos2 = new Vector3(x2, y2, 0);
                var rot2 = Quaternion.Euler(0, 0, 135f);

                SetOrientation(pos2, rot2);
                break;


            case PlayerSpriteState.Up:
            case PlayerSpriteState.IdleUp:
                float x3 = transform.parent.position.x + 0.2f;
                float y3 = transform.parent.position.y + 1.3f;

                var pos3 = new Vector3(x3, y3, 0);
                var rot3 = Quaternion.Euler(0, 0, 45f);

                SetOrientation(pos3, rot3);
                break;


            case PlayerSpriteState.Down:
            case PlayerSpriteState.IdleDown:
                float x4 = transform.parent.position.x + -0.3f;
                float y4 = transform.parent.position.y + -0.3f;

                var pos4 = new Vector3(x4, y4, 0);
                var rot4 = Quaternion.Euler(0, 0, -135f);

                SetOrientation(pos4, rot4);
                break;
        }
    }

    private void SetOrientation(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

}