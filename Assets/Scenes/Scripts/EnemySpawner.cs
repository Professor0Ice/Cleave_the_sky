using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Debuff Settings")]
    public GameObject debuffPrefab;            // префаб дебаффа
    public float debuffChance = 0.1f;          // 10% шанс что вместо врага будет дебафф

    [Header("Wings Booster Settings")]
    public GameObject wingsBoosterPrefab;      // префаб крылышек
    [Range(0f, 0.5f)]
    public float wingsBoosterChance = 0.15f;   // 15% шанс вместо врага (появляются в воздухе)

    [Header("Target")]
    public Transform target;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 25f;     // как далеко впереди заглядывать
    public float minDistanceBetweenGroups = 8f; // минимальное расстояние между группами
    public float maxDistanceBetweenGroups = 15f;// максимальное расстояние
    public float groundY = -4.5f;

    [Header("Air Settings")]
    public bool spawnInAir = true;
    public float minAirHeight = 15f;           // минимальная высота в воздухе
    public float maxAirHeight = 60f;           // максимальная высота
    public float airEnemyChance = 0.35f;       // 35% шанс что враг в воздухе

    [Header("Wings Air Settings")]
    public float wingsMinAirHeight = 20f;       // минимальная высота для крылышек
    public float wingsMaxAirHeight = 50f;       // максимальная высота для крылышек

    [Header("Group Settings")]
    public int enemiesPerGroup = 3;             // количество объектов в группе
    public float groupSpread = 6f;              // разброс объектов в группе

    private float nextSpawnX;
    private float cleanupTimer = 0f;
    private float cleanupInterval = 2f;

    void Start()
    {
        if (target == null)
        {
            GameObject tire = GameObject.FindGameObjectWithTag("Tire");
            if (tire != null) target = tire.transform;
        }

        // Сразу спавним несколько групп впереди
        nextSpawnX = 15f;
        for (int i = 0; i < 5; i++)
        {
            SpawnGroup(nextSpawnX);
            nextSpawnX += Random.Range(minDistanceBetweenGroups, maxDistanceBetweenGroups);
        }
    }

    void Update()
    {
        if (target == null) return;

        // Постоянно проверяем — если впереди мало групп, спавним ещё
        while (target.position.x + spawnDistanceAhead > nextSpawnX)
        {
            SpawnGroup(nextSpawnX);
            nextSpawnX += Random.Range(minDistanceBetweenGroups, maxDistanceBetweenGroups);
        }

        // Очистка старых объектов
        cleanupTimer += Time.deltaTime;
        if (cleanupTimer >= cleanupInterval)
        {
            cleanupTimer = 0f;
            CleanupObjects();
        }
    }

    void SpawnGroup(float centerX)
    {
        for (int i = 0; i < enemiesPerGroup; i++)
        {
            float randomX = centerX + Random.Range(-groupSpread, groupSpread);
            float spawnY;
            bool isWingsBooster = false;
            GameObject objectToSpawn = null;

            // СНАЧАЛА ПРОВЕРЯЕМ КРЫЛЫШКИ (они всегда в воздухе)
            if (wingsBoosterPrefab != null && Random.value < wingsBoosterChance)
            {
                objectToSpawn = wingsBoosterPrefab;
                isWingsBooster = true;
                // Крылышки всегда спавнятся в воздухе
                spawnY = groundY + Random.Range(wingsMinAirHeight, wingsMaxAirHeight);
            }
            // ЕСЛИ НЕ КРЫЛЫШКИ, ТО ПРОВЕРЯЕМ ДЕБАФФ ИЛИ ВРАГА
            else
            {
                // Определяем высоту спавна
                if (spawnInAir && Random.value < airEnemyChance)
                {
                    spawnY = groundY + Random.Range(minAirHeight, maxAirHeight);
                }
                else
                {
                    spawnY = groundY;
                }

                // Выбираем: дебафф или враг
                if (debuffPrefab != null && Random.value < debuffChance)
                {
                    objectToSpawn = debuffPrefab;
                }
                else
                {
                    objectToSpawn = enemyPrefab;
                }
            }

            Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);

            // Логирование для отладки
            if (isWingsBooster)
                Debug.Log($"🪽 Крылышки заспавнены на X={randomX}, Y={spawnY}");
        }
    }

    void CleanupObjects()
    {
        // Очищаем врагов
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            if (obj.transform.position.x < target.position.x - 30f)
            {
                Destroy(obj);
            }
        }

        // Очищаем дебаффы
        GameObject[] debuffs = GameObject.FindGameObjectsWithTag("Debuff");
        foreach (GameObject obj in debuffs)
        {
            if (obj.transform.position.x < target.position.x - 30f)
            {
                Destroy(obj);
            }
        }

        // Очищаем крылышки
        GameObject[] wings = GameObject.FindGameObjectsWithTag("WingsBooster");
        foreach (GameObject obj in wings)
        {
            if (obj.transform.position.x < target.position.x - 30f)
            {
                Destroy(obj);
            }
        }
    }
}