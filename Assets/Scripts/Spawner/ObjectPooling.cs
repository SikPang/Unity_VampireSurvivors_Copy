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

    static ObjectPooling instance;
    Dictionary<string, Queue<GameObject>> poolingDict = new Dictionary<string, Queue<GameObject>>();
    const int initialNumber = 200;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        foreach(CharacterData.CharacterType characterType in Enum.GetValues(typeof(CharacterData.CharacterType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initialNumber; j++)
            {
                newQue.Enqueue(CreateObject(characterType));
            }

            poolingDict.Add(characterType.ToString(), newQue);
        }

        foreach (WeaponData.WeaponType weaponType in Enum.GetValues(typeof(WeaponData.WeaponType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initialNumber; j++)
            {
                newQue.Enqueue(CreateObject(weaponType));
            }

            poolingDict.Add(weaponType.ToString(), newQue);
        }
    }

    static GameObject CreateObject<T>(T type)
    {
        GameObject newObject;

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
                break;
            case WeaponData.WeaponType.Bible:
                newObject = Instantiate(instance.biblePrefab);
                break;
            case WeaponData.WeaponType.Axe:
                newObject = Instantiate(instance.axePrefab);
                break;
            case WeaponData.WeaponType.Pigeon:
                newObject = Instantiate(instance.pigeonPrefab);
                break;
            case WeaponData.WeaponType.Lightning:
                newObject = Instantiate(instance.lightningPrefab);
                break;
            case WeaponData.WeaponType.MagicWand:
                newObject = Instantiate(instance.magicWandPrefab);
                break;
        }

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
}
