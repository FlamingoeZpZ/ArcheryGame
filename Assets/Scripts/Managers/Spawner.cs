using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy spawnObject;
    
    [SerializeField] private float minRespawnPeriod;
    [SerializeField] private float maxRespawnPeriod;
    private float currentSpawnPeriod;

    private void Update()
    {
        currentSpawnPeriod -= Time.deltaTime;
        if (currentSpawnPeriod < 0)
        {
            SpawnEnemy();
        }
    }
    
    private void SpawnEnemy()
    {
        currentSpawnPeriod = Random.Range(minRespawnPeriod, maxRespawnPeriod);
        Instantiate(spawnObject, transform.position, Quaternion.identity);
        
    }
}
