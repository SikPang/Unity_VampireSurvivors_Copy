using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : Weapon
{
    Vector2 destination;
    [SerializeField]Rigidbody2D rigidbody;
    float speed = 300;

    void OnEnable()
    {
        StartCoroutine(StartDestroy());
        destination = EnemySpawner.GetInstance().GetNearestEnemyPosition();
        rigidbody.AddForce((destination - (Vector2)transform.position).normalized * speed, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.GetComponent<Enemy>().ReduceHealthPoint(RandomDamage(attackPower));
            InactiveWeapon();
        }
    }
}