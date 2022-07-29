using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    float attackSpeed;

    void Awake()
    {
        Initialize();
    }

    internal override void Initialize()
    {
        base.Initialize();
        attackSpeed = 100f;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}