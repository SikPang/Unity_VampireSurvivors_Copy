using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /*
     * 성서
     * 비둘기
     * 벼락
     * 마법 지팡이
     * 도끼
     */

    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {

        yield return null;
    }
}