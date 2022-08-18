using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject flyingEyePrefab;
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] GameObject mushroomPrefab;
    [SerializeField] GameObject skeletonPrefab;

    [SerializeField] GameObject whipPrefab;
    [SerializeField] GameObject biblePrefab;
    [SerializeField] GameObject axePrefab;
    [SerializeField] GameObject pigeonPrefab;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] GameObject magicWandPrefab;

    [SerializeField] GameObject blueCrystalPrefab;
    [SerializeField] GameObject greenCrystalPrefab;
    [SerializeField] GameObject redCrystalPrefab;

    [SerializeField] GameObject DamageText;

    static ObjectPooling instance;
    Dictionary<string, Queue<GameObject>> poolingDict = new Dictionary<string, Queue<GameObject>>();

    const int initNumEnemy = 500;
    const int initNumWeapon = 500;
    const int initNumCrystal = 500;
    const int initNumDamage = 500;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        foreach(CharacterData.CharacterType characterType in Enum.GetValues(typeof(CharacterData.CharacterType)))
        {
            if (IsPlayer(characterType)) continue;

            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumEnemy; j++)
            {
                newQue.Enqueue(CreateObject(characterType));
            }

            poolingDict.Add(characterType.ToString(), newQue);
        }

        foreach (WeaponData.WeaponType weaponType in Enum.GetValues(typeof(WeaponData.WeaponType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumWeapon; j++)
            {
                newQue.Enqueue(CreateObject(weaponType));
            }

            poolingDict.Add(weaponType.ToString(), newQue);
        }

        foreach (CrystalData.CrystalType crystalType in Enum.GetValues(typeof(CrystalData.CrystalType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initNumCrystal; j++)
            {
                newQue.Enqueue(CreateObject(crystalType));
            }

            poolingDict.Add(crystalType.ToString(), newQue);
        }

        Queue<GameObject> damageQue = new Queue<GameObject>();

        for (int j = 0; j < initNumDamage; j++)
        {
            damageQue.Enqueue(CreateObject("damage"));
        }

        poolingDict.Add("damage", damageQue);
    }

    static GameObject CreateObject<T>(T type)
    {
        GameObject newObject;
        bool isParentPlayer = false;

        switch (type)
        {
            default:
            case CharacterData.CharacterType.FlyingEye:
                newObject = Instantiate(instance.flyingEyePrefab);
                break;
            case CharacterData.CharacterType.Goblin:
                newObject = Instantiate(instance.goblinPrefab);
                break;
            case CharacterData.CharacterType.Mushroom:
                newObject = Instantiate(instance.mushroomPrefab);
                break;
            case CharacterData.CharacterType.Skeleton:
                newObject = Instantiate(instance.skeletonPrefab);
                break;

            case WeaponData.WeaponType.Whip:
                newObject = Instantiate(instance.whipPrefab);
                if(ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Whip).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.Bible:
                newObject = Instantiate(instance.biblePrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Bible).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.Axe:
                newObject = Instantiate(instance.axePrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Axe).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.FireWand:
                newObject = Instantiate(instance.pigeonPrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.FireWand).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.Lightning:
                newObject = Instantiate(instance.lightningPrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.Lightning).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;
            case WeaponData.WeaponType.MagicWand:
                newObject = Instantiate(instance.magicWandPrefab);
                if (ItemAssets.GetInstance().GetWeaponData(WeaponData.WeaponType.MagicWand).GetParent().Equals(WeaponData.Parent.Player))
                    isParentPlayer = true;
                break;

            case CrystalData.CrystalType.blue:
                newObject = Instantiate(instance.blueCrystalPrefab);
                break;
            case CrystalData.CrystalType.green:
                newObject = Instantiate(instance.greenCrystalPrefab);
                break;
            case CrystalData.CrystalType.red:
                newObject = Instantiate(instance.redCrystalPrefab);
                break;

            case "damage":
                newObject = Instantiate(instance.DamageText);
                break;
        }

        if (isParentPlayer)
            newObject.transform.parent = GameObject.FindWithTag("Weapon").transform;
        else
            newObject.transform.parent = instance.transform;

        newObject.SetActive(false);

        return newObject;
    }

    public static GameObject GetObject<T>(T type)
    {
        if (instance.poolingDict[type.ToString()].Count > 0)
        {
            return instance.poolingDict[type.ToString()].Dequeue();
        }
        else
        {
            return CreateObject(type);
        }
    }

    public static void ReturnObject<T>(GameObject deadEnemy, T type)
    {
        instance.poolingDict[type.ToString()].Enqueue(deadEnemy);
    }

    bool IsPlayer(CharacterData.CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterData.CharacterType.Knight:
            case CharacterData.CharacterType.Bandit:
                return true;
            default:
                return false;
        }
    }
}
