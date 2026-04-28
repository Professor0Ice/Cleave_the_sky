using UnityEngine;

public class TireBounce : MonoBehaviour
{
    public float bounceForce = 15f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (rb == null) return;

            Vector2 direction = (transform.position - other.transform.position).normalized;
            direction.y = Mathf.Max(direction.y + 0.3f, 0.5f);
            direction.Normalize();

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * bounceForce, ForceMode2D.Impulse);

            Debug.Log("БАХ! Отскок от врага. Сила: " + bounceForce);

            Destroy(other.gameObject);
        }
    }
}