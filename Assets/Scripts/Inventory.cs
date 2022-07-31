using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static Inventory instance;
    static Dictionary<WeaponData.WeaponType, WeaponSpawner> inventory;

    void Awake()
    {
        inventory = new Dictionary<WeaponData.WeaponType, WeaponSpawner>();

        GetFirstWeapon();
    }

    void GetFirstWeapon()
    {
        switch (GetComponentInParent<Player>().GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                AddWeapon(GetComponent<WhipSpawner>());
/*                AddWeapon(GetComponent<AxeSpawner>());
                AddWeapon(GetComponent<LightningSpawner>());
                AddWeapon(GetComponent<PigeonSpawner>());
                AddWeapon(GetComponent<BibleSpawner>());
                AddWeapon(GetComponent<MagicWandSpawner>());*/
                break;
            case CharacterData.CharacterType.Bandit:
                AddWeapon(GetComponent<AxeSpawner>());
                break;
        }
    }

    public static Dictionary<WeaponData.WeaponType, WeaponSpawner> GetInventory()
    {
        return inventory;
    }

    public static void AddWeapon(WeaponSpawner weapon)
    {
        if (inventory.ContainsKey(weapon.GetWeaponType()))
        {
            weapon.IncreaseLevel();
        }
        else
        {
            inventory.Add(weapon.GetWeaponType(), weapon);
            weapon.StartWeapon();
        }
    }

    public static void ShowInventory()
    {
        foreach(var weapon in inventory.Values)
        {
            
        }
    }
}