using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    WeaponData weaponData;
    WeaponSpawner.Direction direction;
    public int attackPower;
    protected int level;
    protected float inactiveDelay;

    void OnEnable()
    {
        StartCoroutine(StartDestroy());
    }

    public void SetParameters(WeaponData weaponData , int attackPower, float inactiveDelay, WeaponSpawner.Direction direction)
    {
        this.weaponData = weaponData;
        this.attackPower = attackPower;
        this.inactiveDelay = inactiveDelay;
        this.direction = direction;
    }

    protected WeaponSpawner.Direction GetDirection()
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

    protected virtual IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(inactiveDelay);

        InactiveWeapon();
    }

    protected void InactiveWeapon()
    {
        ObjectPooling.ReturnObject(gameObject, weaponData.GetWeaponType());
        this.gameObject.SetActive(false);
    }

    protected int RandomDamage(int damage)
    {
        int minDamage = (int)(damage * 0.8f);
        int maxDamage = (int)(damage * 1.2f);

        damage = Random.Range(minDamage, maxDamage + 1);

        return damage;
    }
}
