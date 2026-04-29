using UnityEngine;

public class TireSlam : MonoBehaviour
{
    [Header("Slam Settings")]
    public float slamForce = 25f;        // сила удара вниз
    public float cooldown = 1.5f;        // время между использованиями
    public float minHeightToSlam = 2f;   // минимальная высота для слэма (чтобы не под землю)

    private Rigidbody2D rb;
    private DragAndDrop dragAndDrop;
    private float cooldownTimer = 0f;
    private bool canSlam = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dragAndDrop = GetComponent<DragAndDrop>();
    }

    void Update()
    {
        // Слэм только после запуска
        if (dragAndDrop == null || !dragAndDrop.HasLaunched) return;

        // Кулдаун
        if (!canSlam)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canSlam = true;
            }
        }

        // Нажатие Space
        if (Input.GetKeyDown(KeyCode.Space) && canSlam)
        {
            // Проверяем что колесо достаточно высоко
            if (transform.position.y > minHeightToSlam)
            {
                Slam();
            }
        }
    }

    void Slam()
    {
        // Сбрасываем текущую скорость
        rb.linearVelocity = Vector2.zero;

        // Добавляем сильный импульс вниз
        rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);

        canSlam = false;
        cooldownTimer = cooldown;

        Debug.Log("СЛЭМ! Удар вниз с силой: " + slamForce);
    }
}