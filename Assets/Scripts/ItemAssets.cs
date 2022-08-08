using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    [SerializeField] WeaponData whip;
    [SerializeField] WeaponData axe;
    [SerializeField] WeaponData bible;
    [SerializeField] WeaponData magicWand;
    [SerializeField] WeaponData lightning;
    [SerializeField] WeaponData fireWand;

    [SerializeField] AccessoryData armor;
    [SerializeField] AccessoryData clover;
    [SerializeField] AccessoryData crown;
    [SerializeField] AccessoryData emptyTome;
    [SerializeField] AccessoryData spinach;
    [SerializeField] AccessoryData wings;

    static ItemAssets instance;

    void Awake()
    {
        instance = this;
    }

    public static ItemAssets GetInstance()
    {
        return instance;
    }

    public WeaponData GetWeaponData(WeaponData.WeaponType weaponType)
    {
        switch (weaponType)
        {
            default:
            case WeaponData.WeaponType.Axe:
                return axe;
            case WeaponData.WeaponType.Bible:
                return bible;
            case WeaponData.WeaponType.Lightning:
                return lightning;
            case WeaponData.WeaponType.MagicWand:
                return magicWand;
            case WeaponData.WeaponType.FireWand:
                return fireWand;
            case WeaponData.WeaponType.Whip:
                return whip;
        }
    }

    public AccessoryData GetAccessoryData(AccessoryData.AccessoryType type)
    {
        switch (type)
        {
            default:
            case AccessoryData.AccessoryType.Armor:
                return armor;
            case AccessoryData.AccessoryType.Clover:
                return clover;
            case AccessoryData.AccessoryType.Crown:
                return crown;
            case AccessoryData.AccessoryType.EmptyTome:
                return emptyTome;
            case AccessoryData.AccessoryType.Spinach:
                return spinach;
            case AccessoryData.AccessoryType.Wings:
                return wings;
        }
    }
}
