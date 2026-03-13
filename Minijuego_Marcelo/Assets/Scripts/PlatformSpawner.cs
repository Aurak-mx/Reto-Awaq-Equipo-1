using System.Collections;
// using System.Numerics;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformGameObject; 
    public float maxLeft; 
    public float maxRight; 
    public float timeToSpawnMin; 
    public float timeToSpawnMax; 

    public GameObject chestPrefab; 

    [Range(0f, 100f)] // Slider para elijir probabilidad entre 0 y 100 para aparecimiento de cofres
    public float chestSpawnProbability = 20f; // 20% de probabilidad por defecto
    public float chestYOffset = 1f; // Distancia con la cual el cofre aparecera de la plataforma
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnerTime());  
    }

    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(timeToSpawnMin, timeToSpawnMax));

        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(maxLeft, maxRight), transform.position.y, 0); 
        GameObject newPlatform = Instantiate(platformGameObject, spawnPosition, Quaternion.identity);

        if (Random.Range(0f, 100f) <= chestSpawnProbability)
        {
            Vector3 chestPosition = new Vector3(spawnPosition.x, spawnPosition.y + chestYOffset, 0); 
            
            GameObject newChest = Instantiate(chestPrefab, chestPosition, Quaternion.identity);
            
            newChest.transform.SetParent(newPlatform.transform); // Asignar jerarquia con transforms
        }

        StartCoroutine(SpawnerTime()); 

    }
}
