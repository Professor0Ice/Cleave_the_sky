using UnityEngine;

public class TireBounce : MonoBehaviour
{
    [Header("Enemy Bounce")]
    public float bounceForce = 15f;
    public float bounceAngle = 30f;
    public float slamBonusMultiplier = 1.1f;

    [Header("Debuff")]
    public float debuffForce = 15f;        // сила откидывания назад
    public float debuffAngle = 120f;       // угол назад (120°)

    [Header("Booster")]
    public float boosterForce = 700f;
    public float boosterAngle = 90f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Враг - откидывает вперёд-вверх
        if (other.CompareTag("Enemy"))
        {
            if (rb == null) return;

            float angleRad = bounceAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            float finalBounce = bounceForce;

            if (rb.linearVelocity.y < -10f)
            {
                finalBounce = bounceForce * slamBonusMultiplier;
                Debug.Log("СЛЭМ! Бонусный отскок: " + finalBounce);
            }

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * finalBounce, ForceMode2D.Impulse);

            Debug.Log("БАХ! Отскок под " + bounceAngle + "°, сила: " + finalBounce);

            Destroy(other.gameObject);
        }

        // Дебафф - откидывает назад
        if (other.CompareTag("Debuff"))
        {
            if (rb == null) return;

            float angleRad = debuffAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * debuffForce, ForceMode2D.Impulse);

            Debug.Log("ДЕБАФФ! Отскок назад под " + debuffAngle + "°, сила: " + debuffForce);

            Destroy(other.gameObject);
        }

        // Ускоритель
        if (other.CompareTag("Booster"))
        {
            if (rb == null) return;

            float angleRad = boosterAngle * Mathf.Deg2Rad;
            Vector2 boostDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(boostDirection * boosterForce, ForceMode2D.Impulse);

            Debug.Log("УСКОРИТЕЛЬ! Сила: " + boosterForce + " под углом " + boosterAngle + "°");

            Destroy(other.gameObject);
        }
    }
}