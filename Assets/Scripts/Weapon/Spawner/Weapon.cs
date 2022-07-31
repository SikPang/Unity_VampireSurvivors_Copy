using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    int level;
    int attackPower;
    float attackSpeed;

    void Awake()
    {
        Initialize();
    }

    internal void Initialize()
    {
        attackPower = weaponData.GetAttackPower();
        attackSpeed = weaponData.GetAttackSpeed() * Player.GetAttackSpeed() / 100f;
        level = 1;
    }

    public void SpawnWeapon()
    {
        GameObject weapon;

        weapon = ObjectPooling.GetObject(GetWeaponType());

        weapon.transform.position += GetComponentInParent<Player>().GetPosition();
        weapon.SetActive(true);
    }

    public WeaponData.WeaponType GetWeaponType()
    {
        return weaponData.GetWeaponType();
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public int GetLevel()
    {
        return level;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    public void UpdateAttackSpeed()
    {
        attackSpeed = weaponData.GetAttackSpeed() * Player.GetAttackSpeed() / 100f;
    }

    public void AddToInventory()
    {
        Inventory.AddWeapon(this);
    }

    public void StartWeapon()
    {
        StartCoroutine(StartAttack());
    }


    internal abstract IEnumerator StartAttack();
}
