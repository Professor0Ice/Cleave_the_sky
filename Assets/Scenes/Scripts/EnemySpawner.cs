using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Target")]
    public Transform target;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 30f;
    public float distanceBetweenGroups = 40f;
    public float groundY = -4.5f;

    [Header("Group Settings")]
    public int minEnemiesInGroup = 5;
    public int maxEnemiesInGroup = 12;
    public float groupWidth = 8f;

    private float nextSpawnX;
    private float cleanupTimer = 0f;
    private float cleanupInterval = 3f;

    void Start()
    {
        if (target == null)
        {
            GameObject tire = GameObject.FindGameObjectWithTag("Tire");
            if (tire != null) target = tire.transform;
        }

        // Первая группа
        nextSpawnX = 20f;
        SpawnGroup(nextSpawnX);
        nextSpawnX += distanceBetweenGroups;

        // Ещё одна сразу чтобы было что отскакивать
        SpawnGroup(nextSpawnX);
        nextSpawnX += distanceBetweenGroups;
    }

    void Update()
    {
        if (target == null) return;

        // Проверяем не пора ли спавнить новую группу
        if (target.position.x + spawnDistanceAhead > nextSpawnX)
        {
            SpawnGroup(nextSpawnX);
            nextSpawnX += distanceBetweenGroups;
        }

        // Периодическая очистка старых врагов
        cleanupTimer += Time.deltaTime;
        if (cleanupTimer >= cleanupInterval)
        {
            cleanupTimer = 0f;
            CleanupEnemies();
        }
    }

    void SpawnGroup(float centerX)
    {
        int count = Random.Range(minEnemiesInGroup, maxEnemiesInGroup + 1);

        Debug.Log("Спавн группы из " + count + " врагов на X=" + centerX);

        // Один враг точно по центру
        Vector3 centerPos = new Vector3(centerX, groundY, 0f);
        Instantiate(enemyPrefab, centerPos, Quaternion.identity);

        // Остальные случайно в пределах группы
        for (int i = 0; i < count - 1; i++)
        {
            float randomX = centerX + Random.Range(-groupWidth / 2f, groupWidth / 2f);
            Vector3 spawnPos = new Vector3(randomX, groundY, 0f);
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