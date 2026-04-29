using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 10f;      // сила отскока колеса

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tire"))
        {
            Rigidbody2D tireRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (tireRb != null)
            {
                // Направление от врага к колесу
                Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;

                // Добавляем импульс колесу
                tireRb.linearVelocity = Vector2.zero; // сбросить текущую скорость
                tireRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);

                Debug.Log("Колесо отскочило от врага! Сила: " + bounceForce);
            }

            // Уничтожаем врага после столкновения
            Destroy(gameObject);
        }
    }
}