using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class NewBehaviourScript : MonoBehaviour
{

    [Header("Map Tiles")]
    [SerializeField] private GameObject overworldTerrainTile;
    [SerializeField] private GameObject underworldTerrainTile;
    [SerializeField] private GameObject invisibleWall;

    [Header("Objects")] 
    [SerializeField] private GameObject[] keyOjects;

    [Header("Map Parameters")]
    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    [SerializeField] private int spacingOffset = 1;
    [SerializeField] private int smoothingFactor = 5;
    [SerializeField] private int fillPercent = 80;
    
    [Header("Player Objects")]
    [SerializeField] private GameObject player;
    
    private GameObject[,] _overworldTileMap;
    private GameObject[,] _underworldTileMap;
    private GameObject[,] _overworldEnvironmentObjects;
    private GameObject[,] _underworldEnvironmentObjects;
    private Transform _anchor;

    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        _anchor = transform;
        int [,] map = GenerateCellularAutomata(width, height, fillPercent, true, spacingOffset);

        _overworldTileMap = new GameObject[width / spacingOffset, height / spacingOffset];
        _underworldTileMap = new GameObject[width / spacingOffset, height / spacingOffset];
        _overworldEnvironmentObjects = new GameObject[width / spacingOffset, height / spacingOffset];
        _underworldEnvironmentObjects= new GameObject[width / spacingOffset, height / spacingOffset];

        map = SmoothMooreCellularAutomata(map, true, smoothingFactor);
        
        RenderMap(map, _overworldTileMap, _underworldTileMap);
        
        SpawnKeyObjects(map);

        player.transform.position = new Vector3(spawnPoint.x, player.transform.position.y, spawnPoint.z);
    }

    private void RenderMap(int[,] map, GameObject[,] overworldTileMap, GameObject[,] underworldTileMap)
    {
        Vector3 anchorPosition = _anchor.position;
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            for (int z = 0; z < map.GetUpperBound(1); z++)
            {
                
                // 0 = map tile, 1 = invisible wall
                if (map[x, z] == 0)
                {
                    overworldTileMap[x, z] = Instantiate(overworldTerrainTile,
                        new Vector3(x * spacingOffset + anchorPosition.x, 0, z * spacingOffset + anchorPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    GameObject underworldDummyTile = Instantiate(underworldTerrainTile,
                        new Vector3(x * spacingOffset + anchorPosition.x, -1, z * spacingOffset + anchorPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    
                    overworldTileMap[x, z].transform.parent = _anchor;
                    underworldDummyTile.transform.parent = _anchor;
                    
                    underworldTileMap[x, z] = Instantiate(underworldTerrainTile,
                        new Vector3(x * spacingOffset + anchorPosition.x, -100, z * spacingOffset + anchorPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    GameObject overworldDummyTile = Instantiate(overworldTerrainTile,
                        new Vector3(x * spacingOffset + anchorPosition.x, -101, z * spacingOffset + anchorPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    
                    underworldTileMap[x, z].transform.parent = _anchor;
                    overworldDummyTile.transform.parent = _anchor;
                }
                else
                {
                    overworldTileMap[x, z] = Instantiate(invisibleWall,  
                        new Vector3(x * spacingOffset + anchorPosition.x, 0, z * spacingOffset + anchorPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    overworldTileMap[x, z].transform.parent = _anchor;
                }
            }
        }
    }
    
    private static int[,] GenerateCellularAutomata(int width, int height, int fillPercent, bool edgesAreWalls, int spacingOffset)
    {

        int[,] map = new int[width / spacingOffset, height / spacingOffset];

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int z = 0; z < map.GetUpperBound(1); z++)
            {
                if (edgesAreWalls && (x == 0 || x == map.GetUpperBound(0) - 1 || z == 0 || z == map.GetUpperBound(1) - 1))
                {
                    map[x, z] = 1;
                }
                else
                {
                    map[x, z] = (Random.Range(0, 100) < fillPercent) ? 1 : 0;
                }
            }
        }
        return map;
    }
    
    private static int GetMooreSurroundingTiles(int[,] map, int x, int z, bool edgesAreWalls)
    {
        int tileCount = 0;

        for(int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for(int neighbourZ = z - 1; neighbourZ <= z + 1; neighbourZ++)
            {
                if (neighbourX >= 0 && neighbourX < map.GetUpperBound(0) && neighbourZ >= 0 && neighbourZ < map.GetUpperBound(1))
                {
                    //We don't want to count the tile we are checking the surroundings of
                    if(neighbourX != x || neighbourZ != z)
                    {
                        tileCount += map[neighbourX, neighbourZ];
                    }
                }
            }
        }
        return tileCount;
    }
    
    private static int[,] SmoothMooreCellularAutomata(int[,] map, bool edgesAreWalls, int smoothCount)
    {
        for (int i = 0; i < smoothCount; i++)
	    {
		    for (int x = 0; x < map.GetUpperBound(0); x++)
		    {
			    for (int z = 0; z < map.GetUpperBound(1); z++)
			    {
				    int surroundingTiles = GetMooreSurroundingTiles(map, x, z, edgesAreWalls);

                    if (edgesAreWalls && (x == 0 || x == map.GetUpperBound(0) - 1) || z == 0 || z == map.GetUpperBound(1) - 1)
				    {
                        //Set the edge to be a wall if we have edgesAreWalls to be true
					    map[x, z] = 1;
				    }
                    //The default moore rule requires more than 4 neighbours
				    else if (surroundingTiles > 4)
				    {
					    map[x, z] = 1;
				    }
				    else if (surroundingTiles < 4)
                    {
                        map[x, z] = 0;
                    }
                }
		    }
	    }
        return map;
    }

    private void SpawnKeyObjects(int[,] map)
    {
        Vector3 anchorPosition = _anchor.position;
        List<Vector3> validSpawnLocations = GetValidSpawnLocations(map);
;
        spawnPoint = validSpawnLocations[Random.Range(0, validSpawnLocations.Count - 1)];
        validSpawnLocations.Remove(spawnPoint);

        for (int items = 0; items < keyOjects.Length; items++)
        {
            Vector3 location = validSpawnLocations[Random.Range(0, validSpawnLocations.Count - 1)];
            validSpawnLocations.Remove(location);
            
            int coinflip = Random.Range(0, 2);
            if (coinflip == 0)
            {
                _overworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(keyOjects[items],
                    new Vector3(location.x * spacingOffset + anchorPosition.x, 0.75f, 
                        location.z * spacingOffset + anchorPosition.z), Quaternion.Euler(0, 0, 0));
                
                _overworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
            }
            else
            {
                _underworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(keyOjects[items],
                    new Vector3(location.x * spacingOffset + anchorPosition.x, -99.25f, 
                        location.z * spacingOffset + anchorPosition.z), Quaternion.Euler(0, 0, 0));
                
                _underworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
            }
        }
    }

    private List<Vector3> GetValidSpawnLocations(int[,] map)
    {
        int[,] dilutedMap = (int[,]) map.Clone();
        
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int z = 0; z < map.GetUpperBound(1); z++)
            {
                if (dilutedMap[x, z] == 0)
                {
                    if (map[x - 1, z] == 1 || map[x, z - 1] == 1 || map[x + 1, z] == 1 ||
                        map[x, z + 1] == 1)
                    {
                        dilutedMap[x, z] = 1;
                    }
                }
            }
        }

        List<Vector3> validSpawnLocations = new List<Vector3>();

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int z = 0; z < map.GetUpperBound(1); z++)
            {
                if (dilutedMap[x, z] == 0)
                {
                    validSpawnLocations.Add(new Vector3(x, 0.5f, z));
                }
            }
        }

        return validSpawnLocations;
    }
}
