using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    int healthPoint;
    int attackPower;
    int defencePower;
    int speed;

    int maxHealth;

    internal virtual void Initialize()
    {
        healthPoint = characterData.GetHealthPoint();
        attackPower = characterData.GetAttackPower();
        defencePower = characterData.GetDefencePower();
        speed = characterData.GetSpeed();
        maxHealth = characterData.GetHealthPoint();
    }

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

    public Vector3 GetPosition()
    {
        return transform.position;
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

    public CharacterData.CharacterType GetCharacterType()
    {
        return characterData.GetCharacterType();
    }

    public void IncreaseStats()
    {
        //attackPower += increaseAttack;
        //defencePower += increaseDefence;
    }
}