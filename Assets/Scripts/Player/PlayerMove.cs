using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] Character character;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) >= 0.7f && Mathf.Abs(vertical) >= 0.7f)
        {
            horizontal = Mathf.Clamp(horizontal, -0.7f, 0.7f);
            vertical = Mathf.Clamp(vertical, -0.7f, 0.7f);
        }

        rigidBody.MovePosition(rigidBody.position + new Vector2(horizontal, vertical) * character.GetSpeed() * Time.deltaTime);
        rigidBody.velocity = Vector2.zero;
    }
}
