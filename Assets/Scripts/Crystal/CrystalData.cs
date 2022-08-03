using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Crystal Data", order = int.MaxValue)]
public class CrystalData : ScriptableObject
{
    public enum CrystalType
    {
        blue,
        green,
        red
    }

    [SerializeField] Sprite sprite;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] CrystalType crystalType;
    [SerializeField] int expValue;

    public int GetExpValue()
    {
        return expValue;
    }

    public CrystalType GetCristalType()
    {
        return crystalType;
    }

    public RuntimeAnimatorController GetController()
    {
        return controller;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}