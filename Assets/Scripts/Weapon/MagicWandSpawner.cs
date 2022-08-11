using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandSpawner : WeaponSpawner
{
    protected override IEnumerator StartAttack()
    {
        EnemySpawner enemySpawner = EnemySpawner.GetInstance();

        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            if(enemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 2 && enemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 3 && enemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 5 && enemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 7 && enemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }
}