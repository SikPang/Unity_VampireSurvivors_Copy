using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] CrystalData crystalData;
    Sprite sprite;
    int expValue;

    void Awake()
    {
        Initialize();
    }

    internal virtual void Initialize()
    {
        sprite = crystalData.GetSprite();
        expValue = crystalData.GetExpValue();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    public int GetExpValue()
    {
        return expValue;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {

            GetComponent<Rigidbody2D>().AddForce(new Vector2(3f, 0f), ForceMode2D.Force);
        }
    }
}
