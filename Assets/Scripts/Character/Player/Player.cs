using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character
{
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI text;
    static float attackSpeed;
    int maxExpValue;
    int curExpValue;
    int level;

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
        maxExpValue = 50;
        curExpValue = 0;
        level = 1;
        expSlider.maxValue = maxExpValue;
        expSlider.value = curExpValue;
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

    public override void Die()
    {
        Debug.Log("Died");
    }

    public void GetExp(int value)
    {
        if (curExpValue + value >= maxExpValue)
        {
            curExpValue += value - maxExpValue;
            LevelUp();
        }
        else
            curExpValue += value;

        expSlider.value = curExpValue;
    }

    void LevelUp()
    {
        level++;
        text.text = "LV " + level.ToString();

        maxExpValue *= level;
        expSlider.maxValue = maxExpValue;
    }
}