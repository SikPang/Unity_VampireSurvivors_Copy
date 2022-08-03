using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InactiveText());
    }

    IEnumerator InactiveText()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        ObjectPooling.ReturnObject(gameObject, "damage");
        gameObject.SetActive(false);
    }
}
