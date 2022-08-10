using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    static Dictionary<WeaponData.WeaponType, int> weaponInventory;
    static Dictionary<AccessoryData.AccessoryType, int> accesoInventory;

    [SerializeField] Transform weaponSlotTemplate;
    [SerializeField] Transform weaponSlotParent;
    [SerializeField] Transform accessorySlotTemplate;
    [SerializeField] Transform accessorySlotParent;

    RectTransform[] weaponSlots = new RectTransform[slotNum];
    RectTransform[] accessorySlots = new RectTransform[slotNum];

    [SerializeField] Accessory crown;
    [SerializeField] Accessory clover;
    [SerializeField] Accessory armor;
    [SerializeField] Accessory spinach;
    [SerializeField] Accessory emptyTome;
    [SerializeField] Accessory wings;

    WeaponSpawner bible;

    const int slotNum = 6;
    const float slotSize = 30f;

    private Inventory() { }

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        instance = this;
        weaponInventory = new Dictionary<WeaponData.WeaponType, int>();
        accesoInventory = new Dictionary<AccessoryData.AccessoryType, int>();

        bible = GetComponent<BibleSpawner>();

        SlotInitial();
    }

    void SlotInitial()
    {
        for (int i = 0; i < slotNum; i++)
        {
            weaponSlots[i] = Instantiate(weaponSlotTemplate, weaponSlotParent).GetComponent<RectTransform>();
            weaponSlots[i].anchoredPosition = new Vector2(i * slotSize, 0f);
            weaponSlots[i].gameObject.SetActive(true);

            accessorySlots[i] = Instantiate(accessorySlotTemplate, accessorySlotParent).GetComponent<RectTransform>();
            accessorySlots[i].anchoredPosition = new Vector2(i * slotSize, 0f);
            accessorySlots[i].gameObject.SetActive(true);
        }
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
                spawner = bible;
                break;
            case WeaponData.WeaponType.Lightning:
                spawner = GetComponent<LightningSpawner>();
                break;
            case WeaponData.WeaponType.MagicWand:
                spawner = GetComponent<MagicWandSpawner>();
                break;
            case WeaponData.WeaponType.FireWand:
                spawner = GetComponent<FireWandSpawner>();
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

        ShowInventory();
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

        ShowInventory();
    }

    public void ShowInventory()
    {
        int count = 0;
        foreach(WeaponData.WeaponType weapon in weaponInventory.Keys)
        {
            weaponSlots[count].Find("Icon").GetComponent<Image>().sprite = ItemAssets.GetInstance().GetWeaponData(weapon).GetSprite();
            weaponSlots[count].Find("Level").GetComponent<TextMeshProUGUI>().text = weaponInventory[weapon].ToString();
            ++count;
        }

        count = 0;
        foreach (AccessoryData.AccessoryType accessory in accesoInventory.Keys)
        {
            accessorySlots[count].Find("Icon").GetComponent<Image>().sprite = ItemAssets.GetInstance().GetAccessoryData(accessory).GetSprite();
            accessorySlots[count].Find("Level").GetComponent<TextMeshProUGUI>().text = accesoInventory[accessory].ToString();
            ++count;
        }
    }
}