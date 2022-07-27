using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Character Data", order = int.MaxValue )]
public class CharacterData : ScriptableObject
{
    public enum CharacterType
    {
        FlyingEye,
        Goblin,
        Skeleton,
        Mushroom
    }

    [SerializeField] CharacterType characterType;
    [SerializeField] int healthPoint;
    [SerializeField] int attackPower;
    [SerializeField] int defencePower;
    [SerializeField] int speed;

    [SerializeField] int increaseAttack;
    [SerializeField] int increaseDefence;
    
    public int GetHealthPoint()
    {
        return healthPoint;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetDefencePower()
    {
        return defencePower;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public void Die()
    {
        Debug.Log("Die");
    }
}