using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    public enum WeaponType
    {
        Whip,
        Axe,
        Bible,
        Lightning,
        MagicWand,
        FireWand
    }

    public enum Parent
    {
        Self,
        Player
    }

    [SerializeField] Parent parent;
    [SerializeField] WeaponType weaponType;
    [SerializeField] int attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float inactiveDelay;
    [SerializeField] Sprite weaponSprite;
    [SerializeField] string description;
    [SerializeField] Vector2 basePosition;
    [SerializeField] Vector2 baseScale;

    public Parent GetParent()
    {
        return parent;
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetInactiveDelay()
    {
        return inactiveDelay;
    }

    public Sprite GetSprite()
    {
        return weaponSprite;
    }

    public string GetDescription()
    {
        return description;
    }

    public Vector2 GetBasePosition()
    {
        return basePosition;
    }

    public Vector2 GetBaseScale()
    {
        return baseScale;
    }
}
