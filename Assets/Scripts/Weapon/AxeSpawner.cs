using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSpawner : WeaponSpawner
{
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            SpawnWeapon(Direction.Left);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 5)
                SpawnWeapon(Direction.Left);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 7)
                SpawnWeapon(Direction.Right);

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }
}
