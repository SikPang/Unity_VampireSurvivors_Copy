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
            {
                ObjectPooling.ReturnObject(gameObject, crystalData.GetCristalType());
                gameObject.SetActive(false);
                player.GetComponent<Level>().GetExp(expValue);
            }
        }
    }

    IEnumerator CrystalAnimation()
    {
        rigidbody.AddForce(new Vector2(5f * player.GetHorizontal(), 5f * player.GetVertical()), ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(0.5f);

        isCollided=true;
        int speed = 30;

        while (true)
        {
            Vector2 direction = player.transform.position - transform.position;
            rigidbody.MovePosition(rigidbody.position + direction.normalized * Time.deltaTime * speed++);

            yield return null;
        }
    }
}
