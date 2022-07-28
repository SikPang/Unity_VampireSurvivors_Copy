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
        attackSpeed = 1f;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}