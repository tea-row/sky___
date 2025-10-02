using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaclePrefab; 

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
        if (obstaclePrefab == null || startPoint == null)
        {
            Debug.LogError("Prefab or Start Point is not assigned in the LevelGenerator!");
            return;
        }

        for (int i = 0; i < totalRows; i++)
        {
            Vector3 rowStartPosition = startPoint.position + startPoint.forward * i * distanceBetweenRows;

            int obstaclesInThisRow = Random.Range(1, 3); 

            List<int> availableLanes = new List<int>();
            for (int laneIndex = 0; laneIndex < numberOfLanes; laneIndex++)
            {
                availableLanes.Add(laneIndex);
            }

          
            for (int j = 0; j < obstaclesInThisRow; j++)
            {
                int randomLaneIndex = Random.Range(0, availableLanes.Count);
                int chosenLane = availableLanes[randomLaneIndex];

                availableLanes.RemoveAt(randomLaneIndex);

                Vector3 obstaclePosition = rowStartPosition + startPoint.right * chosenLane * laneWidth;

                float randomHorizontalOffset = Random.Range(-horizontalOverlapFactor, horizontalOverlapFactor);
                float randomDepthOffset = Random.Range(-depthOverlapFactor, depthOverlapFactor);
                Vector3 randomOffset = (startPoint.right * randomHorizontalOffset) + (startPoint.forward * randomDepthOffset);

                Vector3 finalObstaclePosition = obstaclePosition + randomOffset;

                Instantiate(obstaclePrefab, finalObstaclePosition, startPoint.rotation, transform);
            }
        }
    }
}