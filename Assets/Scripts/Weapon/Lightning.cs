using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Weapon
{
    void OnEnable()
    {
        StartCoroutine(StartDestroy());
        Vector2 destination = EnemySpawner.GetInstance().GetRandomEnemyPosition();
        Vector2 yAxisAdd = new Vector2(0f, 9f);
        transform.position = destination + yAxisAdd;
    }
}