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
        Pigeon
    }

    [SerializeField] WeaponType weaponType;
    [SerializeField] int attackPower;
    [SerializeField] int attackSpeed;

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetAttackSpeed()
    {
        return attackSpeed;
    }
}
