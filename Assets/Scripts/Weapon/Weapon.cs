using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] internal WeaponSpawner spawner;
    internal int attackPower;
    internal int level;
    internal float inactiveDelay;

    void Awake()
    {
        attackPower = spawner.GetAttackPower();
        level = spawner.GetLevel();
        inactiveDelay = spawner.GetInactiveDelay();
    }

    void Start()
    {
        StartCoroutine(StartDestroy());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            collision.GetComponent<Enemy>().ReduceHealthPoint(RandomDamage(attackPower));
        }
    }

    internal virtual IEnumerator StartDestroy()
    {
        yield return new WaitForSecondsRealtime(inactiveDelay);

        InactiveWeapon();
    }

    internal void InactiveWeapon()
    {
        ObjectPooling.ReturnObject(gameObject, spawner.GetWeaponType());
        this.gameObject.SetActive(false);
    }

    int RandomDamage(int damage)
    {
        int minDamage = (int)(damage * 0.8f);
        int maxDamage = (int)(damage * 1.2f);

        damage = Random.Range(minDamage, maxDamage + 1);
        Debug.Log(damage);

        return damage;
    }
}
