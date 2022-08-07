using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    static Dictionary<WeaponData.WeaponType, int> weaponInventory;
    static Dictionary<AccessoryData.AccessoryType, int> accesoInventory;

    [SerializeField] Accessory crown;
    [SerializeField] Accessory clover;
    [SerializeField] Accessory armor;
    [SerializeField] Accessory spinach;
    [SerializeField] Accessory emptyTome;
    [SerializeField] Accessory wings;

    private Inventory() { }

    void Awake()
    {
        instance = this;
        weaponInventory = new Dictionary<WeaponData.WeaponType, int>();
        accesoInventory = new Dictionary<AccessoryData.AccessoryType, int>();
    }

    private void Update()
    {
        /*sb.Clear();
        foreach(WeaponData.WeaponType type in weaponInventory.Keys)
        {
            sb.Append(type.ToString() + " " + weaponInventory[type] + ", ");
        }
        foreach (AccessoryData.AccessoryType type in accesoInventory.Keys)
        {
            sb.Append(type.ToString() + " " + accesoInventory[type] + ", ");
        }
        Debug.Log(sb.ToString());*/
    }

    public static Inventory GetInstance()
    {
        return instance;
    }

    public static Dictionary<WeaponData.WeaponType, int> GetWeaponInventory()
    {
        return weaponInventory;
    }

    public static Dictionary<AccessoryData.AccessoryType, int> GetAccInventory()
    {
        return accesoInventory;
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
            weaponInventory.Remove(spawner.GetWeaponType());
            weaponInventory.Add(spawner.GetWeaponType(), spawner.GetLevel());
        }
        else
        {
            weaponInventory.Add(spawner.GetWeaponType(), 1);
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
            accesoInventory.Remove(accessory.GetAccessoryType());
            accesoInventory.Add(accessory.GetAccessoryType(), accessory.GetLevel());
            accessory.ApplyEffect();
        }
        else
        {
            accesoInventory.Add(accessory.GetAccessoryType(), 1);
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