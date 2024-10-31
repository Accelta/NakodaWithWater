using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Assign different enemy types here
    public Transform[] spawnPoints;    // The spawn points for enemies
    public float spawnDelay = 5f;      // Delay between spawns

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnDelay);
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }
}
