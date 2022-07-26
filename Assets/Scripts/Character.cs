using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Character Data", order = int.MaxValue )]
public class Character : ScriptableObject
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

    [SerializeField] int maxHealth;

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

    public void ReduceHealthPoint(int damage)
    {
        if(healthPoint <= damage)
        {
            healthPoint = 0;
            Die();
        }
        else
        {
            healthPoint -= damage;
        }
    }

    public void RecoverHealthPoint(int amount)
    {
        if (healthPoint + amount > maxHealth)
        {
            healthPoint = maxHealth;
        }
        else
        {
            healthPoint += amount;
        }
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    public void IncreaseStats()
    {
        attackPower += increaseAttack;
        defencePower += increaseDefence;
    }

    public void Print()
    {
        Debug.Log("healthPoint : " + healthPoint);
        Debug.Log("attackPower : " + attackPower);
        Debug.Log("defencePower : " + defencePower);
        Debug.Log("speed : " + speed);
    }
}