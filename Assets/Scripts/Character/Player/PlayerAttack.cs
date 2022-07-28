using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.AddComponent<Axe>();
        }
    }
}