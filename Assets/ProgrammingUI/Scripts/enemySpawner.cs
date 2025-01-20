using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject prefabToSpawn;

    [Header("Spawn Settings")]
    public Transform spawnPosition;
    public float spawnInterval = 2f;
    public Transform parentObject;

    [Header("Spawn Limit Settings")]
    public int maxEnemies = 7; // Maximum number of enemies allowed
    private int currentEnemyCount = 0;

    private float timeSinceLastSpawn;

    private void Update()
    {
        if (currentEnemyCount < maxEnemies)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnPrefab();
                timeSinceLastSpawn = 0f;
            }
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn != null && spawnPosition != null)
        {
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition.position, spawnPosition.rotation);

            if (parentObject != null)
            {
                spawnedPrefab.transform.SetParent(parentObject);
            }

            // Rotate the prefab randomly on the Y-axis
            float randomYRotation = Random.Range(0, 4) * 90f;
            spawnedPrefab.transform.Rotate(0, randomYRotation, 0);

            currentEnemyCount++;
        }
        else
        {
            Debug.LogWarning("Prefab or spawn position is not assigned!");
        }
    }
}