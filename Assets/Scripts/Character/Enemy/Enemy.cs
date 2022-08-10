using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] CrystalData.CrystalType crystalType;

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        InitHealthPoint();
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public override void ReduceHealthPoint(int damage)
    {
        base.ReduceHealthPoint(damage);

        FloatingDamage(damage);
    }

    void FloatingDamage(int damage)
    {
        GameObject damageText = ObjectPooling.GetObject("damage");
        TextMeshPro textMesh = damageText.GetComponent<TextMeshPro>();
        RectTransform rectTransform = damageText.GetComponent<RectTransform>();

        textMesh.text = damage.ToString();

        rectTransform.position = new Vector3(transform.position.x, transform.position.y + 0.5f ,rectTransform.position.z);

        damageText.SetActive(true);
    }

    public override void Die()
    {
        EnemySpawner.GetInstance().IncreaseKillCount();

        if (Random.Range(0, 10) > 5)
            DropCrystral();

        StartCoroutine(DieAnimation());
    }

    void DropCrystral()
    {
        GameObject crystal = ObjectPooling.GetObject(crystalType);

        crystal.transform.position = transform.position;

        crystal.SetActive(true);
    }

    IEnumerator DieAnimation()
    {
        GetAnimator().SetBool("die", true);
        GetComponent<EnemyMove>().isDead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.2f);

        ObjectPooling.ReturnObject(gameObject, GetCharacterType());
        GetAnimator().SetBool("die", false);
        gameObject.SetActive(false);
    }
}