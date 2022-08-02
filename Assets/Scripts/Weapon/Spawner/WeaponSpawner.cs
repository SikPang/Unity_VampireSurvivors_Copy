using System.Collections;
using UnityEngine;

public abstract class WeaponSpawner : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    int level;
    int attackPower;
    float attackSpeed;
    float inactiveDelay;
    float additionalScale;

    public enum Direction
    {
        Self,
        Opposite,
        Left,
        Right
    }

    void Awake()
    {
        Initialize();
    }

    internal void Initialize()
    {
        attackPower = weaponData.GetAttackPower();
        attackSpeed = weaponData.GetAttackSpeed() * Player.GetAttackSpeed() / 100f;
        inactiveDelay = weaponData.GetInactiveDelay();
        level = 1;
        additionalScale = 100f;
    }

    public virtual void SpawnWeapon(Direction direction)
    {
        GameObject weapon;

        weapon = ObjectPooling.GetObject(GetWeaponType());

        switch (direction)
        {
            case Direction.Self:
                if (GetComponentInParent<PlayerMove>().GetLookingLeft())
                {
                    weapon.transform.position = new Vector3(-weapon.transform.position.x, weapon.transform.position.y, 0f);
                    weapon.GetComponent<SpriteRenderer>().flipX = true;
                }
                break;

            case Direction.Opposite:
                if (!GetComponentInParent<PlayerMove>().GetLookingLeft())
                {
                    weapon.transform.position = new Vector3(-weapon.transform.position.x, weapon.transform.position.y -1f, 0f);
                    weapon.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                    weapon.transform.position = new Vector3(weapon.transform.position.x, weapon.transform.position.y -1f, 0f);
                weapon.GetComponent<SpriteRenderer>().flipY = true;
                break;

            case Direction.Left:
                weapon.transform.position = new Vector3(-weapon.transform.position.x, weapon.transform.position.y, 0f);
                weapon.GetComponent<SpriteRenderer>().flipX = true;
                break;

            case Direction.Right:
                break;
        }

        weapon.transform.position += GetComponentInParent<Player>().GetPosition();
        weapon.transform.localScale = new Vector3(weapon.transform.localScale.x * (additionalScale / 100f), weapon.transform.localScale.y * (additionalScale / 100f), weapon.transform.localScale.z);
        weapon.GetComponent<Weapon>().SetParameters(attackPower,inactiveDelay);

        weapon.SetActive(true);
    }

    public WeaponData.WeaponType GetWeaponType()
    {
        return weaponData.GetWeaponType();
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
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

    public void IncreaseAdditionalScale(float value)
    {
        additionalScale += value;
    }

    public void IncreaseAttackPower(int value)
    {
        attackPower += value;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    public void UpdateAttackSpeed()
    {
        attackSpeed = weaponData.GetAttackSpeed() * Player.GetAttackSpeed() / 100f;
    }

    /*    public void AddToInventory()
        {
            Inventory.AddWeapon(this);
        }*/

    public void StartWeapon()
    {
        StartCoroutine(StartAttack());
    }


    internal abstract IEnumerator StartAttack();
}
