using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static Inventory instance;
    static Dictionary<WeaponData.WeaponType, Weapon> inventory;
    Player player;

    void Awake()
    {
        inventory = new Dictionary<WeaponData.WeaponType,Weapon>();
        player = GetComponent<Player>();

        GetFirstWeapon();
    }

    void GetFirstWeapon()
    {
        switch (player.GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                AddWeapon(WeaponAssets.GetSpawner("Whip").GetComponent<Weapon>());
                break;
            case CharacterData.CharacterType.Bandit:
                AddWeapon(WeaponAssets.GetSpawner("Axe").GetComponent<Weapon>());
                break;
        }
    }

    public static Dictionary<WeaponData.WeaponType,Weapon> GetInventory()
    {
        return inventory;
    }

    public static void AddWeapon(Weapon weapon)
    {
        inventory.Add(weapon.GetWeaponData().GetWeaponType(), weapon);
        weapon.StartWeapon();
    }

    public static void ShowInventory()
    {
        foreach(var weapon in inventory.Values)
        {
            
        }
    }
}