using UnityEngine;

public class PlayerSword : MonoBehaviour
{

    public void UpdateOrientation(PlayerSpriteState state)
    {
        switch(state)
        {
            case PlayerSpriteState.Right:
            case PlayerSpriteState.IdleRight:
                var pos = new Vector3(0.8f, 0.3f, 0);
                var rot = Quaternion.Euler(0, 0, -45f);

                SetOrientation(pos, rot);
                break;


            case PlayerSpriteState.Left:
            case PlayerSpriteState.IdleLeft:
                var pos2 = new Vector3(-0.8f, 0.7f, 0);
                var rot2 = Quaternion.Euler(0, 0, 135f);

                SetOrientation(pos2, rot2);
                break;


            case PlayerSpriteState.Up:
            case PlayerSpriteState.IdleUp:
                var pos3 = new Vector3(0.33f, 1.3f, 0);
                var rot3 = Quaternion.Euler(0, 0, 45f);

                SetOrientation(pos3, rot3);
                break;


            case PlayerSpriteState.Down:
            case PlayerSpriteState.IdleDown:
                var pos4 = new Vector3(-0.3f, -0.3f, 0);
                var rot4 = Quaternion.Euler(0, 0, -135f);

                SetOrientation(pos4, rot4);
                break;
        }
    }

    private void SetOrientation(Vector3 pos, Quaternion rot)
    {
        transform.position = transform.parent.position + pos;
        transform.rotation = rot;
    }

}