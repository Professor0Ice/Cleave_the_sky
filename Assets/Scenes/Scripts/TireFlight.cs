using UnityEngine;

public class TireFlight : MonoBehaviour
{
    [Header("Settings")]
    public float defaultGravityScale = 1f;

    private Rigidbody2D rb;
    private DragAndDrop dragAndDrop;
    private bool isFlying = false;
    private float flightTimer = 0f;
    private float flightSpeed = 0f;
    private Vector2 flightDir = Vector2.right;
    private float originalGravityScale;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dragAndDrop = GetComponent<DragAndDrop>();
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (dragAndDrop != null && !hasLaunched && dragAndDrop.HasLaunched)
        {
            hasLaunched = true;
        }

        if (!hasLaunched) return;

        if (isFlying)
        {
            flightTimer -= Time.deltaTime;

            if (flightTimer <= 0f)
            {
                StopFlight();
            }
            else
            {
                // Принудительное движение в заданном направлении
                rb.linearVelocity = flightDir.normalized * flightSpeed;

                // Фикс вращения — чтобы колесо не крутилось странно
                rb.angularVelocity = 0f;
            }
        }
    }

    public void StartFlight(float duration, float speed, Vector2 direction)
    {
        if (!hasLaunched) return;

        isFlying = true;
        flightTimer = duration;
        flightSpeed = speed;
        flightDir = direction.normalized;

        // Отключаем гравитацию
        rb.gravityScale = 0f;

        // Сбрасываем текущую скорость
        rb.linearVelocity = Vector2.zero;

        // Дополнительно: можно добавить визуальный эффект (частицы, свечение)
        EnableFlightEffect(true);

        Debug.Log("🪽 Полёт активирован! Длительность: " + duration + " сек, скорость: " + speed);
    }

    void StopFlight()
    {
        isFlying = false;

        // Возвращаем гравитацию
        rb.gravityScale = defaultGravityScale;

        // Небольшой импульс вперёд, чтобы не падал камнем
        rb.AddForce(flightDir * 5f, ForceMode2D.Impulse);

        EnableFlightEffect(false);

        Debug.Log("Полёт закончен!");
    }

    void EnableFlightEffect(bool enable)
    {
        // Здесь можно добавить визуальные эффекты
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            if (enable) ps.Play();
            else ps.Stop();
        }
    }

    public bool IsFlying()
    {
        return isFlying;
    }
}