using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public DragAndDrop tireScript;
    public Transform engineTransform;

    [Header("UI")]
    public TMP_Text speedText;
    public TMP_Text distanceText;
    public GameObject restartPanel;
    public TMP_Text finalDistanceText;

    [Header("Settings")]
    public float maxSpeed = 30f;
    public float stopThreshold = 0.3f;
    public float stopCheckDelay = 2f;
    public float pixelsToMeters = 0.5f;

    private bool isLaunched = false;
    private bool isStopped = false;
    private float stopTimer = 0f;
    private Rigidbody2D tireRb;
    private float startX;
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

        if (engineTransform == null)
        {
            GameObject engine = GameObject.Find("Engine");
            if (engine != null) engineTransform = engine.transform;
        }

        if (restartPanel != null)
            restartPanel.SetActive(false);

        if (speedText != null)
            speedText.text = "0 км/ч";

        if (distanceText != null)
            distanceText.text = "0 м";

        if (engineTransform != null)
            startX = engineTransform.position.x;
    }

    void Update()
    {
        if (tireScript == null || tireRb == null) return;

        if (!isLaunched && tireScript.HasLaunched)
        {
            isLaunched = true;
        }

        if (!isLaunched) return;

        // Дистанция
        if (engineTransform != null)
        {
            float currentX = tireScript.transform.position.x;
            distanceTraveled = (currentX - startX) * pixelsToMeters;
            if (distanceTraveled < 0) distanceTraveled = 0;

            if (distanceText != null)
                distanceText.text = distanceTraveled.ToString("F1") + " м";
        }

        // Скорость
        float currentSpeed = tireRb.linearVelocity.magnitude;
        float speedPercent = Mathf.Clamp01(currentSpeed / maxSpeed) * 100f;

        if (speedText != null)
            speedText.text = speedPercent.ToString("F0") + "км/ч";

        // Остановка
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
        if (restartPanel != null)
            restartPanel.SetActive(true);

        if (finalDistanceText != null)
            finalDistanceText.text = "Гордость Индии!\n" + distanceTraveled.ToString("F1") + " м";

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}