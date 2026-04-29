using UnityEngine;

public class TireBounce : MonoBehaviour
{
    public float bounceForce = 15f;
    public float slamBonusMultiplier = 1.5f;
    public float boosterForce = 20f;      // сила ускорителя
    public float boosterAngle = 45f;      // угол запуска ускорителя

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Враг
        if (other.CompareTag("Enemy"))
        {
            if (rb == null) return;

            Vector2 direction = (transform.position - other.transform.position).normalized;
            direction.y = Mathf.Max(direction.y + 0.3f, 0.5f);
            direction.Normalize();

            float finalBounce = bounceForce;
            if (rb.linearVelocity.y < -10f)
            {
                finalBounce = bounceForce * slamBonusMultiplier;
                Debug.Log("СЛЭМ-ПОПАДАНИЕ! Бонусный отскок: " + finalBounce);
            }

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * finalBounce, ForceMode2D.Impulse);

            Debug.Log("БАХ! Отскок от врага. Сила: " + finalBounce);

            Destroy(other.gameObject);
        }

        // Ускоритель
        if (other.CompareTag("Booster"))
        {
            if (rb == null) return;

            // Направление под 45 градусов вперёд-вверх
            float angleRad = boosterAngle * Mathf.Deg2Rad;
            Vector2 boostDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(boostDirection * boosterForce, ForceMode2D.Impulse);

            Debug.Log("УСКОРИТЕЛЬ! Сила: " + boosterForce + " под углом " + boosterAngle + "°");

            Destroy(other.gameObject);
        }
    }
}