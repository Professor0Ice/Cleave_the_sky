using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Target")]
    public Transform target;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 25f;     // как далеко впереди заглядывать
    public float minDistanceBetweenGroups = 8f;  // минимальное расстояние между группами
    public float maxDistanceBetweenGroups = 15f; // максимальное расстояние
    public float groundY = -4.5f;

    [Header("Air Settings")]
    public bool spawnInAir = true;
    public float minAirHeight = 30f;           // минимальная высота в воздухе
    public float maxAirHeight = 60f;          // максимальная высота
    public float airEnemyChance = 0.35f;      // 35% шанс что враг в воздухе

    [Header("Group Settings")]
    public int enemiesPerGroup = 3;           // всегда по 2 врага в группе
    public float groupSpread = 6f;            // разброс врагов в группе (меньше = плотнее)

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

        // Очистка старых врагов
        cleanupTimer += Time.deltaTime;
        if (cleanupTimer >= cleanupInterval)
        {
            cleanupTimer = 0f;
            CleanupEnemies();
        }
    }

    void SpawnGroup(float centerX)
    {
        for (int i = 0; i < enemiesPerGroup; i++)
        {
            // Случайная позиция в пределах группы
            float randomX = centerX + Random.Range(-groupSpread, groupSpread);

            // Высота: земля или воздух
            float spawnY;
            if (spawnInAir && Random.value < airEnemyChance)
            {
                spawnY = groundY + Random.Range(minAirHeight, maxAirHeight);
            }
            else
            {
                spawnY = groundY;
            }

            Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    void CleanupEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.position.x < target.position.x - 30f)
            {
                Destroy(enemy);
            }
        }
    }
}