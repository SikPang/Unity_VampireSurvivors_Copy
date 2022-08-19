using System.Collections;
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
    bool isColliding;

    private Player() { }

    void Awake()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        GameOverWindow.SetActive(false);
        instance = this;
        attackSpeed = 100f;
        expAdditional = 100f;
        luck = 0;
        hpSlider.maxValue = GetHealthPoint();
        hpSlider.value = GetHealthPoint();
        isColliding = false;

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

    public override void ReduceHealthPoint(int damage)
    {
        if (!PlayerMove.GetInstance().isDead)
        {
            base.ReduceHealthPoint(damage);

            hpSlider.value = GetHealthPoint();
            bleeding.Play();

            isColliding = true;

            if (hitCoroutine == null)
                hitCoroutine = StartCoroutine(UnderAttack());
        }
    }

    protected override IEnumerator UnderAttack()
    {
        spriteRenderer.color = Color.red;

        do
        {
            isColliding = false;
            yield return new WaitForSeconds(0.2f);
        }
        while (isColliding);

        spriteRenderer.color = Color.white;
        hitCoroutine = null;
    }
}