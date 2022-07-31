using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandSpawner : WeaponSpawner
{
    internal override IEnumerator StartAttack()
    {
        while (true)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }
}