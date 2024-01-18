using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [Header("Overworld Enemies")] 
    [SerializeField] private GameObject[] overworldEnemies;

    [Header("Underworld Enemies")] 
    [SerializeField] private GameObject[] underworldEnemies;

    [Header("Spawning Parameters")] 
    [SerializeField] private float intervalTimer;

    
    private List<Vector3> _validSpawnPoints;
    private float _timer;
    private int _wave;

    private void Start()
    {
        _wave = 0;
        _timer = intervalTimer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValidSpawnPoints(List<Vector3> spawnPoints)
    {
        _validSpawnPoints = spawnPoints;
    }

    private void Spawn(List<Vector3> validSpawnLocations)
    {
        
    }
}