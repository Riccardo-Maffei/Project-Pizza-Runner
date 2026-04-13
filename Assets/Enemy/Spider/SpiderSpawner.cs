using Utils;
using System;
using UnityEngine;

using Random = UnityEngine.Random;


namespace Enemy.Spider
{
    public class SpiderSpawner : MonoBehaviour
    {
        public GameObject spiderPrefab;
        
        public float spawnRate = 0.3f;

        public int totalSpawnCount = 15;
        private int _currentSpawnCount;
        
        public float spawnDistanceX = 15f; 
        public float[] lanesY = { 0.5f, 1.5f, 2.5f }; 

        public Transform playerTransform;
        
        private Action<bool> _crossObserver;

        private void Start()
        {
            _currentSpawnCount = 0;
            _crossObserver = crossed =>
            {
                if (!crossed) return;

                Debug.Log("Spawner: Player reached finish line!");
                SpawnSpider();
            };

            GameData.CrossedFinishLine.Subscribe(_crossObserver);
        }

        private void OnDestroy()
        {
            GameData.CrossedFinishLine.Unsubscribe(_crossObserver);
        }

        private void SpawnSpider()
        {
            if (playerTransform == null) return;
            
            var randomY = lanesY[Random.Range(0, lanesY.Length)];
            
            // spawn on the right side
            var spawnX = playerTransform.position.x + spawnDistanceX;
            
            var spawnPos = new Vector3(spawnX, randomY, 0f);
            
            Instantiate(spiderPrefab, spawnPos, Quaternion.identity);
            _currentSpawnCount++;
            
            if (_currentSpawnCount < totalSpawnCount) Delay.BySeconds(SpawnSpider, spawnRate);
        }
    }
}
