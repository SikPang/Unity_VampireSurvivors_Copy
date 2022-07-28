using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Weapon
{
    void Start()
    {
        StartCoroutine(StartAttack());
    }

    internal override IEnumerator StartAttack()
    {

        yield return new WaitForSeconds(GetPlayer().GetAttackSpeed());
    }
}