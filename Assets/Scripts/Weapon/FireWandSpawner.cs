using System.Collections;
using UnityEngine;

public class FireWandSpawner : WeaponSpawner
{
    int effectNum = 3;
    float speed = 200f;
    Vector2 destination;

    internal override IEnumerator StartAttack()
    {
        EnemySpawner enemySpawner = EnemySpawner.GetInstance();

        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            if (enemySpawner.GetListCount() > 0)
            {
                destination = enemySpawner.GetRandomEnemyPosition();
                for (int i = 0; i < effectNum; ++i)
                {
                    SpawnWeapon(i);
                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }

    void SpawnWeapon(int i)
    {
        GameObject weapon = ObjectPooling.GetObject(GetWeaponType());
        float destLength = (destination - (Vector2)transform.position).magnitude;
        Vector2 destVector;
        float angle;

        if (GetWeaponData().GetParent().Equals(WeaponData.Parent.Self))
            weapon.transform.position += Player.GetInstance().GetPosition();

        weapon.transform.localScale = new Vector3(weapon.transform.localScale.x * (GetAdditionalScale() / 100f), weapon.transform.localScale.y * (GetAdditionalScale() / 100f), weapon.transform.localScale.z);
        weapon.GetComponent<Weapon>().SetParameters(GetWeaponType(), GetAttackPower(), GetInactiveDelay(), Direction.Self);

        // 여러 갈래로 발사하기 위해 벡터 조절
        if (i == 0 || i % 2 == 0)
            destination.x += i * destLength * 0.25f;
        else
            destination.x -= i * destLength * 0.25f;

        // 목표로의 단위벡터
        destVector = (destination - (Vector2)transform.position).normalized;

        // 이펙트 회전 각 설정
        if (destVector.y < 0)
            angle = -Vector2.Angle(destVector, new Vector2(1, 0));
        else
            angle = Vector2.Angle(destVector, new Vector2(1, 0));

        // 이펙트 회전
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle - 8.5f);

        weapon.SetActive(true);

        // 발사
        weapon.GetComponent<Rigidbody2D>().AddForce(destVector * speed, ForceMode2D.Force);
    }

    public override void LevelUp()
    {
        switch (GetLevel())
        {
            case 3:
                IncreaseAttackPower(10);
                break;
            case 4:
                effectNum++;
                IncreaseAdditionalScale(10f);
                break;
            case 5:
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(10);
                break;
            case 7:
                effectNum++;
                break;
        }
    }
}