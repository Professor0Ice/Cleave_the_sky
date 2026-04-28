using UnityEngine;
using System.Collections.Generic;

public class InfiniteRoad : MonoBehaviour
{
    public GameObject roadPrefab;        // префаб сегмента дороги
    public float segmentLength = 20f;    // длина одного сегмента (должна совпадать с масштабом префаба)
    public int initialSegments = 3;      // сколько сегментов создать в начале
    public float generateAheadDistance = 30f; // за сколько до края начинать генерацию

    private Transform tire;
    private List<GameObject> activeSegments = new List<GameObject>();
    private float lastGeneratedX;         // X координата конца последнего сегмента

    void Start()
    {
        // Находим шину по тегу (тег нужно будет назначить)
        GameObject tireObj = GameObject.FindGameObjectWithTag("Tire");
        if (tireObj != null) tire = tireObj.transform;
        else Debug.LogError("Не найден объект с тегом Tire!");

        // Генерируем начальные сегменты
        for (int i = 0; i < initialSegments; i++)
        {
            float xPos = segmentLength * i;
            GenerateSegment(xPos);
        }
        lastGeneratedX = segmentLength * (initialSegments - 1);
    }

    void Update()
    {
        if (tire == null) return;

        // Если шина приблизилась к концу сгенерированной зоны
        if (tire.position.x + generateAheadDistance > lastGeneratedX)
        {
            GenerateSegment(lastGeneratedX + segmentLength);
            lastGeneratedX += segmentLength;
        }

        // Удаляем сегменты, которые остались далеко позади (экономия)
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i] != null && activeSegments[i].transform.position.x < tire.position.x - segmentLength * 2)
            {
                Destroy(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }

    void GenerateSegment(float xPos)
    {
        Vector3 position = new Vector3(xPos, -5f, 0f); // Y подбери под свою землю
        GameObject newSegment = Instantiate(roadPrefab, position, Quaternion.identity);
        activeSegments.Add(newSegment);
    }
}