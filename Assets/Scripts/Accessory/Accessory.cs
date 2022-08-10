using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : MonoBehaviour
{
    [SerializeField] AccessoryData accessoryData;
    Sprite accessorySprite;
    int level = 1;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        accessorySprite = accessoryData.GetSprite();
    }

    public AccessoryData.AccessoryType GetAccessoryType()
    {
        return accessoryData.GetAccessoryType();
    }

    public Sprite GetSprite()
    {
        return accessorySprite;
    }

    public int GetLevel()
    {
        return level;
    }

    public void IncreaseLevel()
    {
        level++;
        ApplyEffect();
    }

    public void ApplyEffect()
    {
        switch (accessoryData.GetAccessoryType())
        {
            case AccessoryData.AccessoryType.Spinach:
                // 시금치 = 공격력 10% 
                Player.GetInstance().IncreaseAttackPower(10);
                break;
            case AccessoryData.AccessoryType.Crown:
                // 왕관 = 경험치 획득량 8%
                Player.GetInstance().IncreaseExpAdditional(10);
                break;
            case AccessoryData.AccessoryType.Clover:
                // 클로버 = 행운 20%
                Player.GetInstance().IncreaseLuck(20);
                break;
            case AccessoryData.AccessoryType.Wings:
                // 날개 = 이동속도 10%
                Player.GetInstance().IncreaseSpeed(10);
                break;
            case AccessoryData.AccessoryType.Armor:
                // 아머 = 방어력 10%
                Player.GetInstance().IncreaseDefencePower(10);
                break;
            case AccessoryData.AccessoryType.EmptyTome:
                // 빈 책 = 공속 8%
                Player.GetInstance().DecreaseAttackSpeed(8);
                break;
        }
    }
}
