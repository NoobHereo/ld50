using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    public GameObject Host;
    public int Health { get; private set; }
    private bool canMove = false;
    private float speed = 2f;
    private int damage = 0;

    public void Init(GameObject host, int hp, bool canMove, int damage)
    {
        Host = host;
        Health = hp;
        this.canMove = canMove;
        this.damage = damage;
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
            Player.Instance.GainRage(3);
            Destroy(Host.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.Instance.TakeDamage(damage);
        }
    }
}