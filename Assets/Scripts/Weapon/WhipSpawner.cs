using System.Collections;
using UnityEngine;

public class WhipSpawner : WeaponSpawner
{
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            SpawnWeapon(Direction.Self);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Opposite);

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }

    public override void LevelUp()
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
                IncreaseAttackPower(5);
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 7:
                IncreaseAttackPower(10);
                DecreaseAttackSpeed(10f);
                IncreaseAdditionalScale(10f);
                break;
        }
    }
}
