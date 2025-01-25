using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemySpawner : MonoBehaviour
{
    public float startTime;
    public float spawnTime;
    public float positionOffset;
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs;

    void Start()
    {
        InvokeRepeating(nameof(InvokeEnemy), startTime, spawnTime);
    }

    void InvokeEnemy()
    {
        GameObject pickedEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Transform pickedPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(pickedEnemy, pickedPoint.transform.position 
            + new Vector3(Random.Range(-positionOffset, positionOffset),0f, Random.Range(-positionOffset, positionOffset)), 
            Quaternion.Euler(0f,Random.Range(0f,360f),0f));
    }
}
