using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleSpawner : WeaponSpawner
{
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            SpawnWeapon(Direction.Right);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Left);

            if (GetLevel() >= 5)
                SpawnWeapon(Direction.Up);

            if (GetLevel() >= 7)
                SpawnWeapon(Direction.Down);

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }
}