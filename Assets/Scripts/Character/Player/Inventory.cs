using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    static Dictionary<WeaponData.WeaponType, WeaponSpawner> weaponInventory;
    static Dictionary<AccessoryData.AccessoryType, Accessory> accesoInventory;

    [SerializeField] Accessory crown;
    [SerializeField] Accessory clover;
    [SerializeField] Accessory armor;
    [SerializeField] Accessory spinach;
    [SerializeField] Accessory emptyTome;
    [SerializeField] Accessory wings;

    void Awake()
    {
        instance = this;
        weaponInventory = new Dictionary<WeaponData.WeaponType, WeaponSpawner>();
    }

    private Inventory() { }

    public static Inventory GetInstance()
    {
        return instance;
    }

    public static Dictionary<WeaponData.WeaponType, WeaponSpawner> GetInventory()
    {
        return weaponInventory;
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

        if (weaponInventory.ContainsKey(weaponType))
        {
            spawner.IncreaseLevel();
        }
        else
        {
            weaponInventory.Add(spawner.GetWeaponType(), spawner);
            spawner.StartWeapon();
        }
    }

    public void AddAccessory(AccessoryData.AccessoryType accessoryType)
    {
        Accessory accessory;

        switch (accessoryType)
        {
            default:
            case AccessoryData.AccessoryType.Wings:
                accessory = wings;
                break;
            case AccessoryData.AccessoryType.Armor:
                accessory = armor;
                break;
            case AccessoryData.AccessoryType.Clover:
                accessory = clover;
                break;
            case AccessoryData.AccessoryType.EmptyTome:
                accessory = emptyTome;
                break;
            case AccessoryData.AccessoryType.Spinach:
                accessory = spinach;
                break;
            case AccessoryData.AccessoryType.Crown:
                accessory = crown;
                break;
        }

        if (accesoInventory.ContainsKey(accessoryType))
        {
            accessory.IncreaseLevel();
        }
        else
        {
            accesoInventory.Add(accessory.GetAccessoryType(), accessory);
            accessory.ApplyEffect();
        }
    }

    public static void ShowInventory()
    {
        foreach(var weapon in weaponInventory.Values)
        {
            
        }
    }
}