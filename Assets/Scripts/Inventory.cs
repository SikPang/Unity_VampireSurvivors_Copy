using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static Inventory instance;
    static Dictionary<WeaponData.WeaponType, WeaponSpawner> inventory;

    void Awake()
    {
        instance = this;
        inventory = new Dictionary<WeaponData.WeaponType, WeaponSpawner>();
    }

    public static Dictionary<WeaponData.WeaponType, WeaponSpawner> GetInventory()
    {
        return inventory;
    }

    public static void AddWeapon(WeaponData.WeaponType weaponType)
    {
        WeaponSpawner spawner;

        switch (weaponType)
        {
            default:
            case WeaponData.WeaponType.Axe:
                spawner = instance.GetComponent<AxeSpawner>();
                break;
            case WeaponData.WeaponType.Bible:
                spawner = instance.GetComponent<BibleSpawner>();
                break;
            case WeaponData.WeaponType.Lightning:
                spawner = instance.GetComponent<LightningSpawner>();
                break;
            case WeaponData.WeaponType.MagicWand:
                spawner = instance.GetComponent<MagicWandSpawner>();
                break;
            case WeaponData.WeaponType.Pigeon:
                spawner = instance.GetComponent<PigeonSpawner>();
                break;
            case WeaponData.WeaponType.Whip:
                spawner = instance.GetComponent<WhipSpawner>();
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