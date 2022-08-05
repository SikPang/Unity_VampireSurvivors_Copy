using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    static Dictionary<WeaponData.WeaponType, WeaponSpawner> inventory;

    void Awake()
    {
        instance = this;
        inventory = new Dictionary<WeaponData.WeaponType, WeaponSpawner>();
    }

    private Inventory() { }

    public static Inventory GetInstance()
    {
        return instance;
    }

    public static Dictionary<WeaponData.WeaponType, WeaponSpawner> GetInventory()
    {
        return inventory;
    }

    public void AddWeapon(WeaponData.WeaponType weaponType)
    {
        WeaponSpawner spawner;

        switch (weaponType)
        {
            default:
            case WeaponData.WeaponType.Axe:
                spawner = GetComponent<AxeSpawner>();
                break;
            case WeaponData.WeaponType.Bible:
                spawner = GetComponent<BibleSpawner>();
                break;
            case WeaponData.WeaponType.Lightning:
                spawner = GetComponent<LightningSpawner>();
                break;
            case WeaponData.WeaponType.MagicWand:
                spawner = GetComponent<MagicWandSpawner>();
                break;
            case WeaponData.WeaponType.Pigeon:
                spawner = GetComponent<PigeonSpawner>();
                break;
            case WeaponData.WeaponType.Whip:
                spawner = GetComponent<WhipSpawner>();
                break;
        }

        if (inventory.ContainsKey(weaponType))
        {
            spawner.IncreaseLevel();
        }
        else
        {
            inventory.Add(spawner.GetWeaponType(), spawner);
            spawner.StartWeapon();
        }
    }

    public static void ShowInventory()
    {
        foreach(var weapon in inventory.Values)
        {
            
        }
    }
}