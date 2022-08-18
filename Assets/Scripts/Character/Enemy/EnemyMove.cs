using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform player;
    Enemy character;
    SpriteRenderer spriteRenderer;
    public bool isDead;
    Vector2 direction;

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
        direction = (player.position - transform.position).normalized;

        if (direction.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        if (!isDead)
            transform.Translate(direction.normalized * character.GetSpeed()/15f * Time.deltaTime);
    }

    void Initialize()
    {
        character = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = new Vector2();
    }

    public Vector2 GetDirection()
    {
        return direction;
    }
}
