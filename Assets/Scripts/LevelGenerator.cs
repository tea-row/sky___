using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [Range(0f, 1f)]
    public float obstacleSpawnChance = 0.7f; // ปรับค่าตรงนี้ใน Inspector ได้เลย!

    [Header("Coin Settings")]
    [Range(0f, 1f)]
    public float coinSpawnChance = 0.7f;
    public int coinsPerGroup = 5;
    public float distanceBetweenCoins = 1.5f;

    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    [Header("Level Settings")]
    public int totalRows = 50;
    public float distanceBetweenRows = 5f;
    public int numberOfLanes = 3;
    public float laneWidth = 2f;

    [Header("Overlapping Settings")]
    [Tooltip("ระยะสูงสุดที่จะสุ่มตำแหน่งในแนวข้าง (ซ้าย-ขวา) เพื่อให้เกิดการซ้อนทับ")]
    [Range(0f, 2f)]
    public float horizontalOverlapFactor = 0.5f;

    [Tooltip("ระยะสูงสุดที่จะสุ่มตำแหน่งในแนวลึก (หน้า-หลัง)")]
    [Range(0f, 2f)]
    public float depthOverlapFactor = 0.2f;

    [Header("Spawn Area Anchor")]
    public Transform startPoint;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        if (obstaclePrefab == null || startPoint == null || coinPrefab == null)
        {
            Debug.LogError("Prefab or Start Point is not assigned in the LevelGenerator!");
            return;
        }

        for (int i = 0; i < totalRows; i++)
        {
            Vector3 rowStartPosition = startPoint.position + startPoint.forward * i * distanceBetweenRows;

            List<int> availableLanes = new List<int>();
            for (int laneIndex = 0; laneIndex < numberOfLanes; laneIndex++)
            {
                availableLanes.Add(laneIndex);
            }

            // --- ส่วนที่แก้ไขสำหรับ Obstacle ---
            if (Random.value < obstacleSpawnChance)
            {
                int obstaclesToSpawn = Random.Range(1, 3); // สุ่ม 1-2 ชิ้น

                for (int j = 0; j < obstaclesToSpawn; j++)
                {
                    if (availableLanes.Count == 0) break;

                    int randomLaneIndex = Random.Range(0, availableLanes.Count);
                    int chosenLane = availableLanes[randomLaneIndex];
                    availableLanes.RemoveAt(randomLaneIndex);

                    Vector3 obstaclePosition = rowStartPosition + startPoint.right * (chosenLane - (numberOfLanes - 1) * 0.5f) * laneWidth;
                    float randomHorizontalOffset = Random.Range(-horizontalOverlapFactor, horizontalOverlapFactor);
                    float randomDepthOffset = Random.Range(-depthOverlapFactor, depthOverlapFactor);
                    Vector3 randomOffset = (startPoint.right * randomHorizontalOffset) + (startPoint.forward * randomDepthOffset);
                    Vector3 finalObstaclePosition = obstaclePosition + randomOffset;

                    Instantiate(obstaclePrefab, finalObstaclePosition, startPoint.rotation, transform);
                }
            }
            // --- จบส่วนที่แก้ไข ---

            foreach (int emptyLane in availableLanes)
            {
                if (Random.value < coinSpawnChance)
                {
                    Vector3 groupStartPosition = rowStartPosition + startPoint.right * (emptyLane - (numberOfLanes - 1) * 0.5f) * laneWidth;

                    for (int k = 0; k < coinsPerGroup; k++)
                    {
                        Vector3 coinPosition = groupStartPosition + startPoint.forward * k * distanceBetweenCoins;
                        Instantiate(coinPrefab, coinPosition, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}