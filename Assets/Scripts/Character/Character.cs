using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    Sprite sprite;
    RuntimeAnimatorController controller;
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
        sprite = characterData.GetSprite();
        controller = characterData.GetController();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
        GetComponent<Animator>().runtimeAnimatorController = GetController();
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

    public RuntimeAnimatorController GetController()
    {
        return controller;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public virtual void ReduceHealthPoint(int damage)
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
        switch (GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
            case CharacterData.CharacterType.Bandit:
                Debug.Log("Died");
                break;
            default:
                ObjectPooling.ReturnObject(gameObject, GetCharacterType());
                gameObject.SetActive(false);
                break;
        }
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