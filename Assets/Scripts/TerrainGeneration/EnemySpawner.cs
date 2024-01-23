using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Boss Scene")]
    [SerializeField] private bool isBossScene = false;

    [Header("Overworld Enemies")] 
    [SerializeField] private GameObject[] overworldEnemies;

    [Header("Underworld Enemies")] 
    [SerializeField] private GameObject[] underworldEnemies;

    [Header("Spawning Parameters")] 
    [SerializeField] private float intervalTimer;
    [SerializeField] private float intervalTimerDecrease;
    [SerializeField] private int startingGracePeriod;

    [Header("Boss Level")]
    [SerializeField] private float bossInterval; 

    private List<Vector3> _validSpawnPoints;
    private int _mapOffset;
    private Transform _pivot;
    private int _wave;
    private Vector3 _pivotPosition;

    private void Start()
    {
        _wave = 0;
        Debug.Log("Enemy Spawning started");
        StartCoroutine(Spawn());
    }

    public void SetMapData(List<Vector3> spawnPoints, int mapOffset, Transform pivot)
    {
        _validSpawnPoints = spawnPoints;
        _mapOffset = mapOffset;
        _pivot = pivot;
        _pivotPosition = _pivot.position;
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startingGracePeriod);

        if (!isBossScene)
        {
            while (!GameManager.Instance.Escaped)
            {
                ShuffleList(_validSpawnPoints);
                int spawnCounter = _validSpawnPoints.Count;

                spawnCounter = SpawnBosses(spawnCounter, _validSpawnPoints);

                if (spawnCounter > 0)
                {
                    SpawnMobs(spawnCounter, _validSpawnPoints, _validSpawnPoints.Count - spawnCounter);
                }

                yield return new WaitForSeconds(intervalTimer);
                intervalTimer -= intervalTimerDecrease;
            }
        }
        else
        {
            ShuffleList(_validSpawnPoints);
            int spawnCounter = _validSpawnPoints.Count;

            SpawnBoss(_validSpawnPoints);
        }
    }

    private void SpawnBoss(List<Vector3> validSpawnPoints)
    {
        int randint = Random.Range(0, validSpawnPoints.Count);
        Vector3 location = validSpawnPoints[randint];
        NavMeshHit hit;

        Vector3 spawnpoint_overworld = new Vector3(location.x * _mapOffset + _pivotPosition.x, 0,
                    location.z * _mapOffset + _pivotPosition.z);

        if (NavMesh.SamplePosition(spawnpoint_overworld, out hit, 5f, 0))
        {
            spawnpoint_overworld = hit.position;
        }

        GameObject bossEnemy_overworld = Instantiate(overworldEnemies[0], spawnpoint_overworld, Quaternion.Euler(0, 0, 0));
        bossEnemy_overworld.SetActive(true);
        bossEnemy_overworld.transform.parent = transform;

        StartCoroutine(SpawnUnderworld(validSpawnPoints));
    }

    private IEnumerator SpawnUnderworld(List<Vector3> validSpawnPoints)
    {
        yield return new WaitForSeconds(bossInterval);
        int randint = Random.Range(0, validSpawnPoints.Count);
        Vector3 location = validSpawnPoints[randint];
        NavMeshHit hit;


        Vector3 spawnpoint_underworld = new Vector3(location.x * _mapOffset + _pivotPosition.x, -99.5f,
                    location.z * _mapOffset + _pivotPosition.z);

        if (NavMesh.SamplePosition(spawnpoint_underworld, out hit, 5f, 0))
        {
            spawnpoint_underworld = hit.position;
        }

        GameObject bossEnemy_underworld = Instantiate(underworldEnemies[0], spawnpoint_underworld, Quaternion.Euler(0, 0, 0));
        bossEnemy_underworld.SetActive(true);
        bossEnemy_underworld.transform.parent = transform;
    }


    private int SpawnBosses(int spawnCounter, List<Vector3> validSpawnPoints)
    {
        int amount = _wave - 5;
        
        if (amount >= spawnCounter)
        {
            amount = spawnCounter;
        }

        for (int i = amount; i > 0; i--)
        {
            Vector3 location = validSpawnPoints[i];
            NavMeshHit hit;

            int coinflip = Random.Range(0, 2);

            if (coinflip == 0)
            {
                Vector3 spawnpoint = new Vector3(location.x * _mapOffset + _pivotPosition.x, 0, 
                    location.z * _mapOffset + _pivotPosition.z);
                
                if (NavMesh.SamplePosition(spawnpoint, out hit, 5f, 0))
                {
                    spawnpoint = hit.position;
                }
                
                GameObject bossEnemy = Instantiate(overworldEnemies[1], spawnpoint, Quaternion.Euler(0, 0, 0));
                bossEnemy.SetActive(true);
                bossEnemy.transform.parent = transform;
            }
            else
            {
                Vector3 spawnpoint = new Vector3(location.x * _mapOffset + _pivotPosition.x, -99.5f,
                    location.z * _mapOffset + _pivotPosition.z);
                
                if (NavMesh.SamplePosition(spawnpoint, out hit, 5f, 0))
                {
                    spawnpoint = hit.position;
                }
                
                GameObject bossEnemy = Instantiate(underworldEnemies[1], spawnpoint, Quaternion.Euler(0, 0, 0));
                bossEnemy.SetActive(true);
                bossEnemy.transform.parent = transform;
            }

            spawnCounter--;
        }

        return spawnCounter;
    }

    private void SpawnMobs(int spawnCounter, List<Vector3> validSpawnPoints, int index)
    {
        int mushroomAmount = 8 + _wave * 2;
        int cactusAmount = 5 + _wave;

        if (mushroomAmount + cactusAmount > spawnCounter)
        {
            while (mushroomAmount + cactusAmount > spawnCounter)
            {
                mushroomAmount--;
                cactusAmount--;
            }
        }

        NavMeshHit hit;
        
        for (int i = mushroomAmount; i > 0; i--, index++)
        {
            Vector3 location = _validSpawnPoints[index];
            
            Vector3 spawnpoint = new Vector3(location.x * _mapOffset + _pivotPosition.x, 0, 
                location.z * _mapOffset + _pivotPosition.z);
                
            if (NavMesh.SamplePosition(spawnpoint, out hit, 5f, 0))
            {
                spawnpoint = hit.position;
            }
            
            GameObject mushroomEnemy = Instantiate(overworldEnemies[0], spawnpoint, Quaternion.Euler(0, 0, 0));
            mushroomEnemy.SetActive(true);
            mushroomEnemy.transform.parent = transform;
        }
        
        for (int i = cactusAmount; i > 0; i--, index++)
        {
            Vector3 location = _validSpawnPoints[index];
            
            Vector3 spawnpoint = new Vector3(location.x * _mapOffset + _pivotPosition.x, -99.5f,
                location.z * _mapOffset + _pivotPosition.z);
                
            if (NavMesh.SamplePosition(spawnpoint, out hit, 5f, 0))
            {
                spawnpoint = hit.position;
            }
            
            GameObject cactusEnemy = Instantiate(underworldEnemies[0], spawnpoint, Quaternion.Euler(0, 0, 0));
            cactusEnemy.SetActive(true);
            cactusEnemy.transform.parent = transform;
        }
    }
    
    private static void ShuffleList(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}