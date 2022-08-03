using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    void Awake()
    {
        Initialize();
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
}