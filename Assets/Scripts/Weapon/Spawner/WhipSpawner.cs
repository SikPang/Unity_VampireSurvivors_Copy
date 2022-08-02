using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipSpawner : WeaponSpawner
{
    internal override IEnumerator StartAttack()
    {
        while (true)
        {
            if(GetLevel() >= 1)
                SpawnWeapon(Direction.Self);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Opposite);

            if (GetLevel() >= 1)
            {
                IncreaseAttackPower(100);
                IncreaseAdditionalScale(100f);
            }

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }
}
