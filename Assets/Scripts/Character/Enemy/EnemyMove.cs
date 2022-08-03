using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform player;
    Enemy character;
    //Rigidbody2D rigidBody;

    void Awake()
    {
        //rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Enemy>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        //rigidBody.MovePosition(rigidBody.position + (Vector2)direction.normalized * character.GetSpeed() * Time.deltaTime);
        transform.Translate((Vector2)direction.normalized * character.GetSpeed()/10f * Time.deltaTime);
    }
}
