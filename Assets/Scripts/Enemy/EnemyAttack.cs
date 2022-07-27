using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    Character character;
    Coroutine coroutine;

    private void Awake()
    {
        character = GetComponent<CharacterSetting>().GetCharacter();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Attack(collision.gameObject));
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

    IEnumerator Attack(GameObject player)
    {
        while (true)
        {
            player.GetComponent<CharacterSetting>().GetCharacter().ReduceHealthPoint(character.GetAttackPower());
            yield return new WaitForSeconds(0.2f);
        }
    }
}
