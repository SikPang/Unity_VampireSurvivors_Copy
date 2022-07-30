using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    private static Dictionary<WeaponData.WeaponType, Weapon> inventory;

    void Awake()
    {
        inventory = new Dictionary<WeaponData.WeaponType,Weapon>();
    }

    public static void AddWeapon(Weapon weapon)
    {
        inventory.Add(weapon.GetWeaponData().GetWeaponType(), weapon);
    }
}
