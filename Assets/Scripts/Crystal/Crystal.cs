using System.Collections;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] CrystalData crystalData;
    [SerializeField] PlayerMove player;
    Rigidbody2D rigidbody;
    Coroutine coroutine;
    Sprite sprite;
    int expValue;
    bool isCollided;
    int speed;

    void Awake()
    {
        Initialize();
    }

    internal virtual void Initialize()
    {
        sprite = crystalData.GetSprite();
        expValue = crystalData.GetExpValue();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
        rigidbody = GetComponent<Rigidbody2D>();
        isCollided = false;
        speed = 7;
    }

    public int GetExpValue()
    {
        return expValue;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(CrystalAnimation());

            if (isCollided)
                GetCrystal();
        }
    }

    IEnumerator CrystalAnimation()
    {
        rigidbody.AddForce(new Vector2(player.GetHorizontal(), player.GetVertical()) * speed, ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(0.4f);

        isCollided=true;

        StartCoroutine(Disable());

        while (true)
        {
            Vector2 direction = player.transform.position - transform.position;
            rigidbody.MovePosition(rigidbody.position + direction.normalized * Time.deltaTime * speed++);

            yield return null;
        }
    }

    void GetCrystal()
    {
        ObjectPooling.ReturnObject(gameObject, crystalData.GetCristalType());
        gameObject.SetActive(false);
        player.GetComponent<Level>().GetExp((int)(expValue * Player.GetInstance().GetExpAdditional() / 100f));
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.5f);

        GetCrystal();
    }
}
