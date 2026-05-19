using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Setup")]
    public List<GameObject> trackPrefabs;    
    public Transform playerTransform;       
    public Transform trackContainer;        

    [Header("Settings")]
    public float trackLength = 20f;         
    public int initialTracks = 5;           
    public float deleteDistance = 30f;      

    private float _nextSpawnX = 0f;          
    private List<GameObject> _activeTracks = new List<GameObject>();

    void Start()
    {
        // Check Mainmenu endless mode
        if (PlayerPrefs.GetInt("IsEndlessMode", 0) != 1)
        {
            enabled = false; // If not selected
            return;
        }
        
        for (int i = 0; i < initialTracks; i++)
        {
            if (i == 0) SpawnTrack(0); 
            else SpawnTrack(Random.Range(0, trackPrefabs.Count));
        }
    }

    void Update()
    {
        if (playerTransform != null && playerTransform.position.x + (initialTracks * trackLength) > _nextSpawnX)
        {
            SpawnTrack(Random.Range(0, trackPrefabs.Count));
        }

        if (_activeTracks.Count > 0 && playerTransform != null && playerTransform.position.x - _activeTracks[0].transform.position.x > deleteDistance)
        {
            Destroy(_activeTracks[0]);
            _activeTracks.RemoveAt(0);
        }
    }

    void SpawnTrack(int prefabIndex)
    {
        Vector3 spawnPosition = new Vector3(_nextSpawnX, 0f, 0f);
        
        GameObject newTrack = Instantiate(trackPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
        
        // Subobject LoopingTracks
        if (trackContainer != null)
        {
            newTrack.transform.SetParent(trackContainer);
        }

        _activeTracks.Add(newTrack);
        _nextSpawnX += trackLength;
    }
}