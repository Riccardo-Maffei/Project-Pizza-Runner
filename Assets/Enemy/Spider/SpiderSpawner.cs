using UnityEngine;
using Utils;

public class SpiderSpawner : MonoBehaviour
{
    public GameObject spiderPrefab;
    public float spawnRate = 0.3f; // Rate for action
    public float spawnDistanceX = 15f; 
    public float[] lanesY = { 0.5f, 1.5f, 2.5f }; 

    private float _nextSpawnTime;
    private bool _hasStartedSpawning = false;
    private Camera _mainCam;

    void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (GameData.CrossedFinishLine.GetValue())
        {
            Debug.Log("Spawner: Ziellinie erkannt!");
            // set the Time on now, so that the first spider comes.
            if (!_hasStartedSpawning)
            {
                _nextSpawnTime = Time.time; 
                _hasStartedSpawning = true;
                Debug.Log("Ziellinie erreicht! Spawning starts now.");
            }

            if (Time.time >= _nextSpawnTime)
            {
                SpawnSpider();
                _nextSpawnTime = Time.time + spawnRate;
            }
        }
    }

    void SpawnSpider()
    {
        if (spiderPrefab == null || _mainCam == null) return;

        float randomY = lanesY[Random.Range(0, lanesY.Length)];
        // spawn on the right side
        float spawnX = _mainCam.transform.position.x + spawnDistanceX;
        
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);
        Instantiate(spiderPrefab, spawnPos, Quaternion.identity);
    }
}