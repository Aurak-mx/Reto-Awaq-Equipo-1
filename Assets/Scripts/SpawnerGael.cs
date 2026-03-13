using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerGael : MonoBehaviour
{
    [SerializeField] private GameObject[] ObstaclesPrefabs;
    public float obstacleSpawnTime =2f;
    private float timeUnitObstacleSpawn;
    public float obstacleSpeed =1f;


    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeUnitObstacleSpawn += Time.deltaTime;
        if(timeUnitObstacleSpawn >= obstacleSpawnTime)
        {
            Spawn();
            timeUnitObstacleSpawn = 0f;
        }
    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = ObstaclesPrefabs[Random.Range(0, ObstaclesPrefabs.Length)];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.linearVelocity = Vector2.left * obstacleSpeed;
    }
}
