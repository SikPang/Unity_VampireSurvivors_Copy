using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Accessory Data", menuName = "Scriptable Object/Accessory Data", order = int.MaxValue)]
public class AccessoryData : ScriptableObject
{
    public enum AccessoryType
    {
        Spinach,
        Armor,
        EmptyTome,
        Wings,
        Clover,
        Crown
    }

    [SerializeField] AccessoryType accessoryType;
    [SerializeField] Sprite accessorySprite;
    [SerializeField] string description;

    public AccessoryType GetAccessoryType()
    {
        return accessoryType;
    }

    public Sprite GetSprite()
    {
        return accessorySprite;
    }

    public string GetDescription()
    {
        return description;
    }
}
