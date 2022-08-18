using System.Collections;
using UnityEngine;

public abstract class WeaponSpawner : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    int level;
    int attackPower;
    float attackSpeed;
    int finalAttackPower;
    float finalAttackSpeed;
    float inactiveDelay;
    float additionalScale;
    Sprite weaponIcon;

    public enum Direction
    {
        Self,
        Opposite,
        Left,
        Right,
        Up,
        Down
    }

    void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        weaponIcon = weaponData.GetSprite();
        attackPower = weaponData.GetAttackPower();
        attackSpeed = weaponData.GetAttackSpeed();
        inactiveDelay = weaponData.GetInactiveDelay();
        level = 1;
        additionalScale = 100f;
    }

    public virtual void SpawnWeapon(Direction direction)
    {
        GameObject weapon= ObjectPooling.GetObject(GetWeaponType());

        weapon.transform.localPosition = weaponData.GetBasePosition();
        weapon.transform.localScale = weaponData.GetBaseScale();

        switch (direction)
        {
            case Direction.Self:
                if (PlayerMove.GetInstance().GetLookingLeft())
                {
                    weapon.transform.localPosition = new Vector3(-weapon.transform.localPosition.x, weapon.transform.localPosition.y, 0f);
                    weapon.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                    weapon.GetComponent<SpriteRenderer>().flipX = false;
                weapon.GetComponent<SpriteRenderer>().flipY = false;
                break;

            case Direction.Opposite:
                if (!PlayerMove.GetInstance().GetLookingLeft())
                {
                    weapon.transform.localPosition = new Vector3(-weapon.transform.localPosition.x, weapon.transform.localPosition.y - 1f, 0f);
                    weapon.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x, weapon.transform.localPosition.y - 1f, 0f);
                    weapon.GetComponent<SpriteRenderer>().flipX = false;
                }
                weapon.GetComponent<SpriteRenderer>().flipY = true;
                break;

            case Direction.Left:
                weapon.transform.localPosition = new Vector3(-weapon.transform.localPosition.x, weapon.transform.localPosition.y, 0f);
                weapon.GetComponent<SpriteRenderer>().flipX = true;
                weapon.GetComponent<SpriteRenderer>().flipY = false;
                break;

            case Direction.Right:
                weapon.GetComponent<SpriteRenderer>().flipX = false;
                weapon.GetComponent<SpriteRenderer>().flipY = false;
                break;

            case Direction.Up:
                weapon.transform.localPosition = new Vector3(0f, weapon.transform.localPosition.y + 3f, 0f);
                weapon.GetComponent<SpriteRenderer>().flipX = false;
                weapon.GetComponent<SpriteRenderer>().flipY = false;
                break;

            case Direction.Down:
                weapon.transform.localPosition = new Vector3(0f, weapon.transform.localPosition.y - 3.5f, 0f);
                weapon.GetComponent<SpriteRenderer>().flipX = false;
                weapon.GetComponent<SpriteRenderer>().flipY = false;
                break;
        }

        if (weaponData.GetParent().Equals(WeaponData.Parent.Self))
            weapon.transform.position += Player.GetInstance().GetPosition();

        weapon.transform.localScale = new Vector2(weaponData.GetBaseScale().x * (additionalScale / 100f), weaponData.GetBaseScale().y * (additionalScale / 100f));
        weapon.GetComponent<Weapon>().SetParameters(weaponData, finalAttackPower,inactiveDelay, direction);

        weapon.SetActive(true);
    }

    public WeaponData.WeaponType GetWeaponType()
    {
        return weaponData.GetWeaponType();
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public int GetAttackPower()
    {
        return finalAttackPower;
    }

    public float GetAttackSpeed()
    {
        return finalAttackSpeed;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetInactiveDelay()
    {
        return inactiveDelay;
    }

    public float GetAdditionalScale()
    {
        return additionalScale;
    }

    public Sprite GetSprite()
    {
        return weaponIcon;
    }

    public void IncreaseAdditionalScale(float value)
    {
        additionalScale += value;
    }

    public void IncreaseAttackPower(int value)
    {
        attackPower += value;
    }

    public void DecreaseAttackSpeed(float value)
    {
        attackSpeed -= attackSpeed * value / 100f;
    }

    public void IncreaseLevel()
    {
        level++;
        LevelUp();
    }

    public void UpdateAttackSpeed()
    {
        finalAttackSpeed = attackSpeed * Player.GetInstance().GetAttackSpeed() / 100f;
    }

    public void UpdateAttackPower()
    {
        finalAttackPower = attackPower * Player.GetInstance().GetAttackPower() / 100;
    }

    public void StartWeapon()
    {
        StartCoroutine(StartAttack());
    }

    public virtual void LevelUp()
    {
        switch (GetLevel())
        {
            case 3:
                IncreaseAttackPower(5);
                break;
            case 4:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 5:
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(10);
                break;
        }
    }

    protected abstract IEnumerator StartAttack();
}
