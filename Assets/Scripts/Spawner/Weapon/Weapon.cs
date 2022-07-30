using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    Player player;
    int level;
    int attackPower;
    float attackSpeed;

    void Awake()
    {
        Initialize();
    }

    internal void Initialize()
    {
        player = GetComponent<Player>();
        attackPower = weaponData.GetAttackPower();
        attackSpeed = weaponData.GetAttackSpeed() * (GetPlayer().GetAttackSpeed() / 100f);
        level = 1;
    }

    public void SpawnWeapon()
    {
        GameObject weapon;

        weapon = ObjectPooling.GetObject(weaponData.GetWeaponType());

        weapon.SetActive(true);
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public Player GetPlayer()
    {
        return player;
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
        attackSpeed = weaponData.GetAttackSpeed() * (GetPlayer().GetAttackSpeed() / 100f);
    }
    

    internal abstract IEnumerator StartAttack();
}
