using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSpawner : WeaponSpawner
{
    internal override IEnumerator StartAttack()
    {
        while (true)
        {
            SpawnWeapon(Direction.Self);
            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }

    public override void LevelUp()
    {
        IncreaseLevel();

        switch (GetLevel())
        {
            case 3:
                IncreaseAttackPower(100);
                IncreaseAdditionalScale(100f);
                DecreaseAttackSpeed(100f);
                break;
        }
    }
}
