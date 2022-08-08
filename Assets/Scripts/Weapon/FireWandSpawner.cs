using System.Collections;
using UnityEngine;

public class FireWandSpawner : WeaponSpawner
{
    int num = 3;
    float speed = 200f;
    Vector2 destination;

    internal override IEnumerator StartAttack()
    {
        EnemySpawner enemySpawner = EnemySpawner.GetInstance();

        while (true)
        {
            //Debug.Log(GetAttackPower());

            UpdateAttackPower();
            UpdateAttackSpeed();

            if (enemySpawner.GetListCount() > 0)
            {
                destination = enemySpawner.GetNearestEnemyPosition() * 3f;
                for (int i = 0; i < num; ++i)
                    SpawnWeapon(i, destination);
            }

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }

    public override void LevelUp()
    {
        IncreaseLevel();

        Debug.Log("levelUp");

        switch (GetLevel())
        {
            case 2:
                num++;
                break;
            case 3:
                IncreaseAttackPower(5);
                break;
            case 4:
                num++;
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 5:
                DecreaseAttackSpeed(10f);
                break;
        }
    }

    void SpawnWeapon(int i, Vector2 destination)
    {
        GameObject weapon;

        weapon = ObjectPooling.GetObject(GetWeaponType());

        if (GetWeaponData().GetParent().Equals(WeaponData.Parent.Self))
            weapon.transform.position += Player.GetInstance().GetPosition();

        weapon.transform.localScale = new Vector3(weapon.transform.localScale.x * (GetAdditionalScale() / 100f), weapon.transform.localScale.y * (GetAdditionalScale() / 100f), weapon.transform.localScale.z);
        weapon.GetComponent<Weapon>().SetParameters(GetAttackPower(), GetInactiveDelay(), Direction.Self);

        weapon.SetActive(true);

        if (i == 0 || i % 2 == 0)
            destination.x += i;
        else
            destination.x -= i;

        weapon.GetComponent<Rigidbody2D>().AddForce((destination - (Vector2)transform.position*3f).normalized * speed, ForceMode2D.Force);
    }
}