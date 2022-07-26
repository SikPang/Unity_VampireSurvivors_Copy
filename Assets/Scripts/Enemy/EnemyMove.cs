using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] Transform player;
    [SerializeField] Character character;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        rigidBody.MovePosition(rigidBody.position + (Vector2)direction.normalized * character.GetSpeed() * Time.deltaTime);

        // 밀리는거 해결
    }
}
