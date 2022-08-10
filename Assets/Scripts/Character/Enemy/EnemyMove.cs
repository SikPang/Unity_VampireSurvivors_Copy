using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform player;
    Enemy character;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    public bool isDead;

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        isDead = false;
    }

    void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        if(!isDead)
            rigidBody.MovePosition(rigidBody.position + direction * character.GetSpeed() * Time.deltaTime);

        if (direction.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        //transform.Translate((Vector2)direction.normalized * character.GetSpeed()/10f * Time.deltaTime);
    }

    void Initialize()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
