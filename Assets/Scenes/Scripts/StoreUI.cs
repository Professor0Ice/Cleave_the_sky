using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [Header("Main References")]
    public GameObject storePanel;           // главная панель магазина
    public GameObject restartPanel;         // ссылка на панель рестарта
    public Button openStoreButton;          // кнопка "Магазин" на панели рестарта
    public Button closeStoreButton;         // кнопка "Назад" в магазине

    [Header("Store Items")]
    public StoreItemUI[] storeItems;        // массив товаров

    [Header("Currency")]
    public TMP_Text coinsText;              // текст с количеством монет
    public int startingCoins = 9999;        // начальное количество монет (для шоукейса)
    private int totalCoins;

    [Header("Game Components (для применения улучшений)")]
    public WingsBooster wingsBoosterPrefab;     // ссылка на префаб крылышек
    public TireBounce tireBounce;               // ссылка на скрипт отскоков колеса
    public MinigameController minigameController; // ссылка на мини-игру

    void Start()
    {
        // Для шоукейса — сразу много монет
        totalCoins = startingCoins;
        UpdateCoinsUI();

        // Скрываем магазин при старте
        if (storePanel != null)
            storePanel.SetActive(false);

        // Назначаем кнопки
        if (openStoreButton != null)
            openStoreButton.onClick.AddListener(OpenStore);

        if (closeStoreButton != null)
            closeStoreButton.onClick.AddListener(CloseStore);

        // Инициализируем товары и загружаем улучшения
        foreach (StoreItemUI item in storeItems)
        {
            // Сбрасываем уровни при старте (для шоукейса)
            item.currentLevel = 0;
            item.currentValue = 0f;

            if (item.buyButton != null)
            {
                item.buyButton.onClick.AddListener(() => TryBuyItem(item));
            }
            UpdateItemUI(item);
        }
    }

    void OpenStore()
    {
        if (storePanel != null)
            storePanel.SetActive(true);

        if (restartPanel != null)
            restartPanel.SetActive(false);

        // Обновляем UI
        foreach (StoreItemUI item in storeItems)
        {
            UpdateItemUI(item);
        }

        Time.timeScale = 0f;
    }

    void CloseStore()
    {
        if (storePanel != null)
            storePanel.SetActive(false);

        if (restartPanel != null)
            restartPanel.SetActive(true);
    }

    void TryBuyItem(StoreItemUI item)
    {
        int currentPrice = GetCurrentPrice(item);

        if (totalCoins >= currentPrice)
        {
            totalCoins -= currentPrice;
            UpdateCoinsUI();

            // Улучшаем предмет
            UpgradeItem(item);

            Debug.Log($"Куплено: {item.itemName}! Уровень: {item.currentLevel}");
        }
        else
        {
            Debug.Log($"Не хватает монет для {item.itemName}! Нужно: {currentPrice}, есть: {totalCoins}");
        }
    }

    void UpgradeItem(StoreItemUI item)
    {
        item.currentLevel++;
        item.currentValue += item.upgradeStep;

        // Применяем улучшение в зависимости от типа
        switch (item.itemType)
        {
            case ItemType.WingsDuration:
                if (wingsBoosterPrefab != null)
                    wingsBoosterPrefab.flightDuration = 3f + item.currentValue;
                Debug.Log($"🪽 Длительность полёта: {3f + item.currentValue} сек");
                break;

            case ItemType.WingsSpeed:
                if (wingsBoosterPrefab != null)
                    wingsBoosterPrefab.flightSpeed = 30f + item.currentValue;
                Debug.Log($"🪽 Скорость полёта: {30f + item.currentValue}");
                break;

            case ItemType.BoosterForce:
                if (tireBounce != null)
                    tireBounce.boosterForce = 700f + item.currentValue;
                Debug.Log($"⚡ Сила бустера: {700f + item.currentValue}");
                break;

            case ItemType.StartForce:
                if (minigameController != null)
                    minigameController.baseForce = 15f + item.currentValue;
                Debug.Log($"🚀 Сила запуска: {15f + item.currentValue}");
                break;
        }

        // Обновляем UI
        UpdateItemUI(item);
    }

    void UpdateItemUI(StoreItemUI item)
    {
        if (item.nameText != null)
            item.nameText.text = item.itemName;

        if (item.levelText != null)
            item.levelText.text = $"Ур. {item.currentLevel}";

        if (item.valueText != null)
            item.valueText.text = $"+{item.currentValue:F1}";

        if (item.priceText != null)
            item.priceText.text = $"{GetCurrentPrice(item)}";

        if (item.buyButton != null)
        {
            item.buyButton.interactable = totalCoins >= GetCurrentPrice(item);
        }
    }

    int GetCurrentPrice(StoreItemUI item)
    {
        // Цена растёт с каждым уровнем
        return item.basePrice + (item.currentLevel * item.priceIncrement);
    }

    void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = $"{totalCoins}";
    }
}

public enum ItemType
{
    WingsDuration,   // длительность полёта крылышек
    WingsSpeed,      // скорость полёта
    BoosterForce,    // сила бустера
    StartForce       // сила запуска из пушки
}

[System.Serializable]
public class StoreItemUI
{
    public string itemName;
    public ItemType itemType;

    [Header("Upgrade Values")]
    public int currentLevel = 0;
    public float currentValue = 0f;
    public float upgradeStep = 0.5f;

    [Header("Price")]
    public int basePrice = 50;
    public int priceIncrement = 25;

    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text valueText;
    public TMP_Text priceText;
    public Button buyButton;
}