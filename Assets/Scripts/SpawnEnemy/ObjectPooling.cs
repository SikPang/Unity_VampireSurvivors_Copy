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

    static ObjectPooling instance;
    Dictionary<CharacterData.CharacterType, Queue<GameObject>> poolingDict = new Dictionary<CharacterData.CharacterType,Queue<GameObject>>();
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
                newQue.Enqueue(CreateEnemy(characterType));
            }

            poolingDict.Add(characterType, newQue);
        }
    }

    static GameObject CreateEnemy(CharacterData.CharacterType characterType)
    {
        GameObject newEnemy;
        switch (characterType)
        {
            default:
            case CharacterData.CharacterType.FlyingEye:
                newEnemy = Instantiate(instance.flyingEyePrefab);
                break;
            case CharacterData.CharacterType.Goblin:
                newEnemy = Instantiate(instance.goblinPrefab);
                break;
            case CharacterData.CharacterType.Mushroom:
                newEnemy = Instantiate(instance.mushroomPrefab);
                break;
            case CharacterData.CharacterType.Skeleton:
                newEnemy = Instantiate(instance.skeletonPrefab);
                break;
        }

        newEnemy.transform.parent = instance.transform;
        newEnemy.SetActive(false);

        return newEnemy;
    }

    public static GameObject GetEnemy(CharacterData.CharacterType characterType)
    {
        if (instance.poolingDict[characterType].Count > 0)
        {
            return instance.poolingDict[characterType].Dequeue();
        }
        else
        {
            return CreateEnemy(characterType);
        }
    }

    public static void ReturnEnemy(GameObject deadEnemy, CharacterData.CharacterType characterType)
    {
        instance.poolingDict[characterType].Enqueue(deadEnemy);
    }
}
