using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssets : MonoBehaviour
{
    static WeaponAssets instance;

    [SerializeField] List<GameObject> prefabList = new List<GameObject>();
    [SerializeField] List<Weapon> spawnerList = new List<Weapon>();

    Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
    Dictionary<string, Weapon> spawnerDict = new Dictionary<string, Weapon>();

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        foreach (GameObject prefab in prefabList)
            prefabDict.Add(prefab.name.ToString(), prefab);

        foreach (Weapon weapon in spawnerList)
            spawnerDict.Add(weapon.GetWeaponData().GetWeaponType().ToString(), weapon);
    }

    public static GameObject GetPrefab(string name)
    {
        return instance.prefabDict[name];
    }

    public static GameObject GetSpawner(string name)
    {
        return instance.spawnerDict[name].gameObject;
    }
}
