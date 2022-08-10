using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] Slider hpSlider;
    static Player instance;
    float attackSpeed;
    float expAdditional;
    int luck;
    bool check = false;

    private Player() {}

    void Awake()
    {
        Initialize();

        StartCoroutine(LevelUpTest());
    }

    void Update()
    {
        hpSlider.value = GetHealthPoint();

        if (Input.GetKeyDown(KeyCode.Space))
            check = true;
    }

    internal override void Initialize()
    {
        base.Initialize();
        instance = this;
        attackSpeed = 100f;
        expAdditional = 100f;
        luck = 0;
        hpSlider.maxValue = GetHealthPoint();
        hpSlider.value = GetHealthPoint();
        GetFirstWeapon();
    }

    public static Player GetInstance()
    {
        return instance;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetExpAdditional()
    {
        return expAdditional;
    }

    public int GetLuck()
    {
        return luck;
    }

    public void DecreaseAttackSpeed(float value)
    {
        attackSpeed -= value;
    }

    public void IncreaseExpAdditional(float value)
    {
        expAdditional += value;
    }

    public void IncreaseLuck(int value)
    {
        luck += value;
    }

    void GetFirstWeapon()
    {
        switch (GetComponentInParent<Player>().GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Bible);
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

    IEnumerator LevelUpTest()
    {
        while (true)
        {
            if (check) break;

            yield return new WaitForSeconds((float)Level.GetPlayerLevel());

            GetComponent<Level>().GetExp(50*Level.GetPlayerLevel());
        }
    }
}