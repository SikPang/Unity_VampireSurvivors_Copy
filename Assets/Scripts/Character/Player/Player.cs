using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] Slider hpSlider;
    static float attackSpeed;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        hpSlider.value = GetHealthPoint();
    }

    internal override void Initialize()
    {
        base.Initialize();
        attackSpeed = 100f;
        hpSlider.maxValue = GetHealthPoint();
        hpSlider.value = GetHealthPoint();
        GetFirstWeapon();
    }

    public static float GetAttackSpeed()
    {
        return attackSpeed;
    }

    void GetFirstWeapon()
    {
        switch (GetComponentInParent<Player>().GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Whip);
                break;
            case CharacterData.CharacterType.Bandit:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Axe);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
            GetComponent<SpriteRenderer>().color = Color.red;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    
}