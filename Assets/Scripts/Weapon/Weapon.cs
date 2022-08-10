using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //[SerializeField] internal WeaponSpawner spawner;
    WeaponSpawner.Direction direction;
    WeaponData.WeaponType weaponType;
    public int attackPower;
    internal int level;
    internal float inactiveDelay;

    void Start()
    {
        StartCoroutine(StartDestroy());
    }

    public void SetParameters(WeaponData.WeaponType weaponType , int attackPower, float inactiveDelay, WeaponSpawner.Direction direction)
    {
        this.weaponType = weaponType;
        this.attackPower = attackPower;
        this.inactiveDelay = inactiveDelay;
        this.direction = direction;
    }

    internal WeaponSpawner.Direction GetDirection()
    {
        return direction;
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
        yield return new WaitForSeconds(inactiveDelay);

        InactiveWeapon();
    }

    internal void InactiveWeapon()
    {
        ObjectPooling.ReturnObject(gameObject, weaponType);
        this.gameObject.SetActive(false);
    }

    internal int RandomDamage(int damage)
    {
        int minDamage = (int)(damage * 0.8f);
        int maxDamage = (int)(damage * 1.2f);

        damage = Random.Range(minDamage, maxDamage + 1);

        return damage;
    }
}
