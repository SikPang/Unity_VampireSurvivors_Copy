using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Enemy character;
    Coroutine coroutine;

    void Awake()
    {
        character = GetComponent<Enemy>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (coroutine == null && gameObject.activeSelf)
            {
                coroutine = StartCoroutine(Attack());
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
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

    IEnumerator Attack()
    {
        while (true)
        {
            GiveDamage();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void GiveDamage()
    {
        int damage = character.GetAttackPower() - (int)(character.GetAttackPower() * Player.GetInstance().GetDefencePower()/100f);

        Player.GetInstance().ReduceHealthPoint(damage);
    }
}