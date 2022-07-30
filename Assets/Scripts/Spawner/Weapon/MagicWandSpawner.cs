using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandSpawner : Weapon
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