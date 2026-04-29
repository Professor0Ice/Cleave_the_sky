using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public DragAndDrop tireScript;
    public Transform engineTransform;    // позиция двигателя

    [Header("UI")]
    public TMP_Text speedText;
    public TMP_Text distanceText;        // текст дистанции
    public GameObject restartButton;

    [Header("Settings")]
    public float maxSpeed = 30f;
    public float stopThreshold = 0.3f;
    public float stopCheckDelay = 2f;
    public float pixelsToMeters = 0.5f;  // коэффициент перевода пикселей в метры

    private bool isLaunched = false;
    private bool isStopped = false;
    private float stopTimer = 0f;
    private Rigidbody2D tireRb;
    private float startX;                // позиция двигателя по X
    private float distanceTraveled = 0f;

    void Start()
    {
        if (tireScript == null)
        {
            GameObject tire = GameObject.FindGameObjectWithTag("Tire");
            if (tire != null)
            {
                tireScript = tire.GetComponent<DragAndDrop>();
                tireRb = tire.GetComponent<Rigidbody2D>();
            }
        }
        else
        {
            tireRb = tireScript.GetComponent<Rigidbody2D>();
        }

        // Находим двигатель если не назначен
        if (engineTransform == null)
        {
            GameObject engine = GameObject.Find("Engine");
            if (engine != null) engineTransform = engine.transform;
        }

        if (restartButton != null)
            restartButton.SetActive(false);

        if (speedText != null)
            speedText.text = "Скорость: 0%";

        if (distanceText != null)
            distanceText.text = "0 м";

        if (engineTransform != null)
            startX = engineTransform.position.x;
    }

    void Update()
    {
        if (tireScript == null || tireRb == null) return;

        // Проверяем флаг запуска
        if (!isLaunched && tireScript.HasLaunched)
        {
            isLaunched = true;
            Debug.Log("КОЛЕСО ЗАПУЩЕНО! Начинаем отслеживание.");
        }

        if (!isLaunched) return;

        // Считаем дистанцию от двигателя
        if (engineTransform != null)
        {
            float currentX = tireScript.transform.position.x;
            distanceTraveled = (currentX - startX) * pixelsToMeters;

            // Дистанция не может быть отрицательной
            if (distanceTraveled < 0) distanceTraveled = 0;

            if (distanceText != null)
            {
                distanceText.text = distanceTraveled.ToString("F1") + " м";
            }
        }

        // Отображение скорости
        float currentSpeed = tireRb.linearVelocity.magnitude;
        float speedPercent = Mathf.Clamp01(currentSpeed / maxSpeed) * 100f;

        if (speedText != null)
        {
            speedText.text = "Скорость: " + speedPercent.ToString("F0") + "%";
        }

        // Проверка остановки
        if (currentSpeed < stopThreshold)
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= stopCheckDelay && !isStopped)
            {
                isStopped = true;
                OnTireStopped();
            }
        }
        else
        {
            stopTimer = 0f;
            isStopped = false;
        }
    }

    void OnTireStopped()
    {
        Debug.Log("КОЛЕСО ОСТАНОВИЛОСЬ! Показываем рестарт. Дистанция: " + distanceTraveled.ToString("F1") + " м");

        if (restartButton != null)
            restartButton.SetActive(true);

        // Показываем финальную дистанцию на кнопке или отдельно
        if (distanceText != null)
        {
            distanceText.text = "Дистанция: " + distanceTraveled.ToString("F1") + " м";
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}