using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    [Header("Booster Prefab")]
    public GameObject boosterPrefab;

    [Header("Target")]
    public Transform target;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 40f;
    public float distanceBetweenBoosters = 40f;
    public float groundY = -4.5f;

    private float nextSpawnX;

    void Start()
    {
        if (target == null)
        {
            GameObject tire = GameObject.FindGameObjectWithTag("Tire");
            if (tire != null) target = tire.transform;
        }

        nextSpawnX = 40f;
        SpawnBooster(nextSpawnX);
        nextSpawnX += distanceBetweenBoosters;
    }

    void Update()
    {
        if (target == null) return;

        if (target.position.x + spawnDistanceAhead > nextSpawnX)
        {
            SpawnBooster(nextSpawnX);
            nextSpawnX += distanceBetweenBoosters;
        }

        // Очистка старых
        GameObject[] boosters = GameObject.FindGameObjectsWithTag("Booster");
        foreach (GameObject booster in boosters)
        {
            if (booster.transform.position.x < target.position.x - 30f)
            {
                Destroy(booster);
            }
        }
    }

    void SpawnBooster(float xPos)
    {
        Vector3 spawnPos = new Vector3(xPos, groundY, 0f);
        Instantiate(boosterPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Ускоритель спавнен на X=" + xPos);
    }
}