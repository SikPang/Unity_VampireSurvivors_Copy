using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bible : Weapon
{
    [SerializeField] Transform player;
    const float speed = 200f;

    private void Update()
    {
        transform.RotateAround(player.transform.position, new Vector3(0f, 0f, 1f), speed * Time.deltaTime);
        transform.Rotate(new Vector3(0f, 0f, -1f), speed * Time.deltaTime);
    }
}