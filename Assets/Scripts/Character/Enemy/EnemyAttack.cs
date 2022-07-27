using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    Enemy character;
    Coroutine coroutine;

    private void Awake()
    {
        character = GetComponent<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Attack(collision.gameObject.GetComponent<Player>()));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }

    IEnumerator Attack(Player player)
    {
        while (true)
        {
            GiveDamage(player);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void GiveDamage(Player player)
    {
        int damage = character.GetAttackPower() - player.GetDefencePower();

        player.ReduceHealthPoint(damage);
    }
}