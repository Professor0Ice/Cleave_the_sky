using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject minigamePanel;
    public Slider slider;
    public RectTransform needle;
    public TMP_Text minigameText;
    public TMP_Text valueText;

    [Header("Slider Settings")]
    public float sliderSpeed = 1.5f;
    public float sliderAcceleration = 0.3f;

    [Header("Angle Settings")]
    public float minAngle = 25f;
    public float maxAngle = 80f;

    [Header("Speed Coefficients")]
    public float[] speedCoefficients = { 0.9f, 1.5f, 2.0f };

    [Header("Launch Settings")]
    public float baseForce = 15f;

    private enum MinigamePhase { Hidden, Angle, Speed }
    private MinigamePhase currentPhase = MinigamePhase.Hidden;

    private float currentSliderValue = 0.5f;
    private float currentSpeed;
    private bool movingRight = true;

    private float chosenAngle;
    private float chosenCoefficient;

    private Engine currentEngine;
    private DragAndDrop currentTire;

    private float sliderWidth;
    private bool isActive = false;
    private float inputDelay = 0.3f;      // задержка перед приёмом ввода
    private float delayTimer = 0f;

    void Start()
    {
        if (minigamePanel != null)
            minigamePanel.SetActive(false);

        if (slider != null)
            sliderWidth = slider.GetComponent<RectTransform>().sizeDelta.x;

        Debug.Log("MinigameController готов. width=" + sliderWidth);
    }

    public void StartMinigame(Engine engine, DragAndDrop tire)
    {
        Debug.Log("StartMinigame вызван!");
        currentEngine = engine;
        currentTire = tire;
        isActive = true;

        currentPhase = MinigamePhase.Angle;

        // Сбрасываем таймер задержки
        delayTimer = inputDelay;

        if (minigamePanel != null)
        {
            minigamePanel.SetActive(true);
            Debug.Log("Панель показана");
        }

        if (minigameText != null)
        {
            minigameText.text = "Выбери УГОЛ: нажми E";
            Debug.Log("Текст обновлён: Выбери УГОЛ");
        }
        else
        {
            Debug.LogError("minigameText не назначен!");
        }

        currentSliderValue = 0.5f;
        currentSpeed = sliderSpeed;
        movingRight = true;
    }

    void Update()
    {
        if (!isActive || currentPhase == MinigamePhase.Hidden) return;

        // Отсчитываем задержку
        if (delayTimer > 0f)
        {
            delayTimer -= Time.unscaledDeltaTime;
        }

        // Движение ползунка
        float delta = currentSpeed * Time.unscaledDeltaTime;
        if (movingRight)
        {
            currentSliderValue += delta;
            if (currentSliderValue >= 1f)
            {
                currentSliderValue = 1f;
                movingRight = false;
            }
        }
        else
        {
            currentSliderValue -= delta;
            if (currentSliderValue <= 0f)
            {
                currentSliderValue = 0f;
                movingRight = true;
            }
        }

        currentSpeed += sliderAcceleration * Time.unscaledDeltaTime;

        if (slider != null)
            slider.value = currentSliderValue;

        if (needle != null)
        {
            float needleX = Mathf.Lerp(-sliderWidth / 2f, sliderWidth / 2f, currentSliderValue);
            needle.anchoredPosition = new Vector2(needleX, needle.anchoredPosition.y);
        }

        // Обновляем текст значения
        if (currentPhase == MinigamePhase.Angle)
        {
            float angle = Mathf.Lerp(minAngle, maxAngle, currentSliderValue);
            if (valueText != null)
                valueText.text = angle.ToString("F0") + "°";
        }
        else if (currentPhase == MinigamePhase.Speed)
        {
            int index = Mathf.RoundToInt(currentSliderValue * (speedCoefficients.Length - 1));
            float coeff = speedCoefficients[index];
            if (valueText != null)
                valueText.text = "x" + coeff.ToString("F1");
        }

        // Ждём нажатия E (только после задержки!)
        if (Input.GetKeyDown(KeyCode.E) && delayTimer <= 0f)
        {
            Debug.Log("E нажата в фазе: " + currentPhase);

            if (currentPhase == MinigamePhase.Angle)
            {
                chosenAngle = Mathf.Lerp(minAngle, maxAngle, currentSliderValue);
                Debug.Log("Угол выбран: " + chosenAngle);

                currentPhase = MinigamePhase.Speed;
                if (minigameText != null)
                    minigameText.text = "Выбери СКОРОСТЬ: нажми E";

                currentSliderValue = 0.5f;
                currentSpeed = sliderSpeed;
                movingRight = true;

                // Снова задержка чтобы не проскочило
                delayTimer = inputDelay;
            }
            else if (currentPhase == MinigamePhase.Speed)
            {
                int index = Mathf.RoundToInt(currentSliderValue * (speedCoefficients.Length - 1));
                chosenCoefficient = speedCoefficients[index];
                Debug.Log("Скорость выбрана: x" + chosenCoefficient + ", ЗАПУСК!");

                LaunchTire();
                isActive = false;
                currentPhase = MinigamePhase.Hidden;

                if (minigamePanel != null)
                    minigamePanel.SetActive(false);
            }
        }
    }

    void LaunchTire()
    {
        float angleRad = chosenAngle * Mathf.Deg2Rad;
        float finalForce = baseForce * chosenCoefficient;

        float forceX = Mathf.Cos(angleRad) * finalForce;
        float forceY = Mathf.Sin(angleRad) * finalForce;

        Vector2 launchForce = new Vector2(forceX, forceY);

        Debug.Log("Запуск! Угол: " + chosenAngle + "°, Коэф: x" + chosenCoefficient + ", Сила: " + launchForce);

        if (currentTire != null)
        {
            currentTire.Launch(launchForce);
        }

        if (currentEngine != null)
        {
            currentEngine.TireLaunched();
        }
    }
}