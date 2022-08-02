using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleSpawner : WeaponSpawner
{
    internal override IEnumerator StartAttack()
    {
        while (true)
        {
            SpawnWeapon(Direction.Self);
            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }
}