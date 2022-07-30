using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    static float attackSpeed;

    void Awake()
    {
        Initialize();
    }

    internal override void Initialize()
    {
        base.Initialize();
        attackSpeed = 100f;
    }

    public static float GetAttackSpeed()
    {
        return attackSpeed;
    }
}