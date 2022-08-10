using UnityEngine;

public class Axe : Weapon
{
    [SerializeField] Rigidbody2D rigidbody;
    const float speed = 10f;
    int hitCount;

    void OnEnable()
    {
        StartCoroutine(StartDestroy());
        hitCount = 0;
        Throw();
    }

    void Throw()
    {
        switch (GetDirection())
        {
            case WeaponSpawner.Direction.Self:
                if (PlayerMove.GetInstance().GetLookingLeft())
                    rigidbody.AddForce(new Vector2(-0.3f, 1) * speed, ForceMode2D.Impulse);
                else
                    rigidbody.AddForce(new Vector2(0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
            case WeaponSpawner.Direction.Opposite:
                if (PlayerMove.GetInstance().GetLookingLeft())
                    rigidbody.AddForce(new Vector2(0.3f, 1) * speed, ForceMode2D.Impulse);
                else
                    rigidbody.AddForce(new Vector2(-0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
            case WeaponSpawner.Direction.Left:
                rigidbody.AddForce(new Vector2(-0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
            case WeaponSpawner.Direction.Right:
                rigidbody.AddForce(new Vector2(0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
        }

        rigidbody.AddTorque(speed / 2, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.GetComponent<Enemy>().ReduceHealthPoint(RandomDamage(attackPower));
            if (hitCount >= 1)
                InactiveWeapon();
            hitCount++;
        }
    }
}