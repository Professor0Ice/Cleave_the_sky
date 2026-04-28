using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public DragAndDrop tireScript;       // ссылка на DragAndDrop колеса

    [Header("UI")]
    public TMP_Text speedText;
    public GameObject restartButton;

    [Header("Settings")]
    public float maxSpeed = 30f;
    public float stopThreshold = 0.3f;
    public float stopCheckDelay = 2f;

    private bool isLaunched = false;
    private bool isStopped = false;
    private float stopTimer = 0f;
    private Rigidbody2D tireRb;

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

        if (restartButton != null)
            restartButton.SetActive(false);

        if (speedText != null)
            speedText.text = "Скорость: 0%";
    }

    void Update()
    {
        if (tireScript == null || tireRb == null) return;

        // Проверяем флаг из DragAndDrop
        if (!isLaunched && tireScript.HasLaunched)
        {
            isLaunched = true;
            Debug.Log("КОЛЕСО ЗАПУЩЕНО! Начинаем отслеживание.");
        }

        if (!isLaunched) return;

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
        Debug.Log("КОЛЕСО ОСТАНОВИЛОСЬ! Показываем рестарт.");

        if (restartButton != null)
            restartButton.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}