using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object Pooling
public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject flyingEyePrefab;
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] GameObject mushroomPrefab;
    [SerializeField] GameObject skeletonPrefab;

    static ObjectPooling instance;
    Dictionary<Character.CharacterType, Queue<GameObject>> poolingDict = new Dictionary<Character.CharacterType,Queue<GameObject>>();
    const int initialNumber = 200;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        foreach(Character.CharacterType characterType in Enum.GetValues(typeof(Character.CharacterType)))
        {
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < initialNumber; j++)
            {
                newQue.Enqueue(CreateEnemy(characterType));
            }

            poolingDict.Add(characterType, newQue);
        }
    }

    static GameObject CreateEnemy(Character.CharacterType characterType)
    {
        GameObject newEnemy;
        switch (characterType)
        {
            default:
            case Character.CharacterType.FlyingEye:
                newEnemy = Instantiate(instance.flyingEyePrefab);
                break;
            case Character.CharacterType.Goblin:
                newEnemy = Instantiate(instance.goblinPrefab);
                break;
            case Character.CharacterType.Mushroom:
                newEnemy = Instantiate(instance.mushroomPrefab);
                break;
            case Character.CharacterType.Skeleton:
                newEnemy = Instantiate(instance.skeletonPrefab);
                break;
        }
        newEnemy.SetActive(false);

        return newEnemy;
    }

    public static GameObject GetEnemy(Character.CharacterType characterType)
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

    public static void ReturnEnemy(GameObject deadEnemy, Character.CharacterType characterType)
    {
        instance.poolingDict[characterType].Enqueue(deadEnemy);
    }
}
