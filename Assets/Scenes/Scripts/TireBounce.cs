using UnityEngine;

public class TireBounce : MonoBehaviour
{
    public float bounceForce = 25f;         // увеличена базовая сила отскока
    public float bounceAngle = 30f;         // фиксированный угол отскока от врага
    public float slamBonusMultiplier = 1.5f;

    [Header("Booster")]
    public float boosterForce = 35f;        // увеличена сила бустера
    public float boosterAngle = 30f;        // угол бустера

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Враг - откидывает под 30 градусов вперёд-вверх
        if (other.CompareTag("Enemy"))
        {
            if (rb == null) return;

            // Фиксированное направление под углом bounceAngle
            float angleRad = bounceAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            float finalBounce = bounceForce;

            // Слэм-бонус при падении с большой скоростью
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

        // Ускоритель - мощный бросок
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