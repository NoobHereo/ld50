using System.Collections;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private bool animating = false;

    public void UpdateOrientation(PlayerSpriteState state)
    {
        if (!animating)
        {
            //gameObject.GetComponent<Outline>().enabled = false;
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            switch (state)
            {
                case PlayerSpriteState.Right:
                case PlayerSpriteState.IdleRight:
                    var pos = new Vector3(0.55f, 0.45f, 0);
                    var rot = Quaternion.Euler(0, 0, -45f);

                    SetOrientation(pos, rot);
                    break;


                case PlayerSpriteState.Left:
                case PlayerSpriteState.IdleLeft:
                    var pos2 = new Vector3(-0.59f, 0.45f, 0);
                    var rot2 = Quaternion.Euler(0, 0, 135f);

                    SetOrientation(pos2, rot2);
                    break;


                case PlayerSpriteState.Up:
                case PlayerSpriteState.IdleUp:
                    var pos3 = new Vector3(0.035f, 1.06f, 0);
                    var rot3 = Quaternion.Euler(0, 0, 45f);

                    SetOrientation(pos3, rot3);
                    break;


                case PlayerSpriteState.Down:
                case PlayerSpriteState.IdleDown:
                    var pos4 = new Vector3(0.085f, -0.06f, 0);
                    var rot4 = Quaternion.Euler(0, 0, -135f);

                    SetOrientation(pos4, rot4);
                    break;
            }
        }        
    }

    private void SetOrientation(Vector3 pos, Quaternion rot)
    {
        transform.position = transform.parent.position + pos;
        transform.rotation = rot;
    }

    public void AnimateSwing(PlayerSpriteState state)
    {
        if (!animating)
        {
            animating = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            switch (state)
            {
                case PlayerSpriteState.Right:
                case PlayerSpriteState.IdleRight:
                    ExeAnim(270f, 445f);
                    break;


                case PlayerSpriteState.Left:
                case PlayerSpriteState.IdleLeft:
                    ExeAnim(370f, 545f);
                    break;


                case PlayerSpriteState.Up:
                case PlayerSpriteState.IdleUp:
                    ExeAnim(330f, 505f);
                    break;


                case PlayerSpriteState.Down:
                case PlayerSpriteState.IdleDown:
                    ExeAnim(595f, 770f);
                    break;
            }
        }
    }

    private void ExeAnim(float start, float end)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, start);
        StartCoroutine(Swing(start, end));
    }

    private IEnumerator Swing(float start, float end)
    {
        for (float z = start; z < end; z += 5f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, start + z);
            yield return null;
        }

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        animating = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && animating)
        {
            collision.GetComponent<EnemyDamageTrigger>().TakeDamage(Player.Instance.Damage);
            return;
        }
    }
}