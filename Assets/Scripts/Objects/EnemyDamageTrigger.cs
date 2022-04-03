using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    public GameObject Host;
    public int Health { get; private set; } 

    public void Init(GameObject host, int hp)
    {
        Host = host;
        Health = hp;
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