using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab;
    public Transform player;
    
    public float spawnDistance = 20f;
    public float groundLength;
    private float lastSpawnX;

    void Start()
    {
        Renderer rend = groundPrefab.GetComponentInChildren<Renderer>();
        groundLength = rend.bounds.size.x;

        lastSpawnX = transform.position.x;

        for(int i = 0; i < 30; i++)
        {
            SpawnGround();
        }
    }

    void Update()
    {
        if (player.position.x + spawnDistance > lastSpawnX)
        {
            SpawnGround();
        }
    }

    void SpawnGround()
    {
        Instantiate(groundPrefab, new Vector3(lastSpawnX, 0, 0), Quaternion.identity);
        lastSpawnX += groundLength;
    }
}