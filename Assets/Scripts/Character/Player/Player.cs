using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    static float attackSpeed;

    void Awake()
    {
        Initialize();
    }

    internal override void Initialize()
    {
        base.Initialize();
        attackSpeed = 100f;
        GetFirstWeapon();
    }

    public static float GetAttackSpeed()
    {
        return attackSpeed;
    }

    void GetFirstWeapon()
    {
        switch (GetComponentInParent<Player>().GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Whip);
                break;
            case CharacterData.CharacterType.Bandit:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Axe);
                break;
        }
    }
}