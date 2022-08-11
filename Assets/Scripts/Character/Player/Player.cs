using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] Slider hpSlider;
    [SerializeField] ParticleSystem bleeding;
    [SerializeField] GameObject GameOverWindow;
    static Player instance;
    float attackSpeed;
    float expAdditional;
    int luck;
    bool check = false;     // 테스트용

    private Player() {}

    void Awake()
    {
        Initialize();

        //StartCoroutine(LevelUpTest());
    }

    void Update()   
    {
        if (Input.GetKeyDown(KeyCode.Space))    // 테스트용
            check = true;
    }

    protected override void Initialize()
    {
        GameOverWindow.SetActive(false);
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

    public override void ReduceHealthPoint(int damage)
    {
        if (!PlayerMove.GetInstance().isDead)
        {
            base.ReduceHealthPoint(damage);

            hpSlider.value = GetHealthPoint();
            bleeding.Play();
        }
    }

    public override void Die()
    {
        PlayerMove.GetInstance().isDead = true;
        StartCoroutine(DieAnimation());
    }

    protected override IEnumerator DieAnimation()
    {
        GetAnimator().SetBool("Death", true);

        yield return new WaitForSeconds(1f);

        GameOverWindow.SetActive(true);
        Time.timeScale = 0f;
        // 게임 오버 창
    }

    void GetFirstWeapon()
    {
        switch (GetComponentInParent<Player>().GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.FireWand);
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

    IEnumerator LevelUpTest()   // 테스트용
    {
        while (true)
        {
            if (check) break;

            yield return new WaitForSeconds((float)Level.GetPlayerLevel());

            GetComponent<Level>().GetExp(50*Level.GetPlayerLevel());
        }
    }
}