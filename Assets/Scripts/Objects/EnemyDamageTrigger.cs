using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    public GameObject Host;
    public int Health { get; private set; }
    private bool canMove = false;
    private float speed = 2f;

    public void Init(GameObject host, int hp, bool canMove)
    {
        Host = host;
        Health = hp;
        this.canMove = canMove;
    }

    private void Update()
    {
        if (Vector2.Distance(Host.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 3f && canMove)
            Host.transform.position = Vector2.MoveTowards(Host.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, speed * Time.deltaTime);
        else
            Host.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void TakeDamage(int dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            World.Instance.EnemyDeath();
            Destroy(Host.gameObject);
        }
    }
}