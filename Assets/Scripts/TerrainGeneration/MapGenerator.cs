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
    [SerializeField] private GameObject[] overworldEnvironmentObjects;
    [SerializeField] private GameObject[] underworldEnvironmentObjects;
    [SerializeField] private int treeOptionCount;
    [SerializeField] private int plantOptionCount;
    [SerializeField] private int rockOptionCount;
    [SerializeField] private int treeDensityPercent;
    [SerializeField] private int plantDensityPercent;
    [SerializeField] private int rockDensityPercent;


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

    [System.Serializable]
    public class Element
    {

        public string name;
        [Range(1, 10)] public int density;

        public GameObject[] prefabs;

        public bool CanPlace()
        {

            // Validation check to see if element can be placed. More detailed calculations can go here, such as checking perlin noise.

            if (Random.Range(0, 10) < density)
                return true;
            else
                return false;

        }

        public GameObject GetRandom()
        {

            // Return a random GameObject prefab from the prefabs array.

            return prefabs[Random.Range(0, prefabs.Length)];
        }
    }

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
        
        List<Vector3> validSpawnLocations = new List<Vector3>();
        
        validSpawnLocations = SpawnKeyObjects(map, validSpawnLocations);

        player.transform.position = new Vector3(spawnPoint.x, player.transform.position.y, spawnPoint.z);
        
        PopulateMap(validSpawnLocations);
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

    private List<Vector3> SpawnKeyObjects(int[,] map, List<Vector3> validSpawnLocations)
    {
        Vector3 anchorPosition = _anchor.position;
        validSpawnLocations = GetValidSpawnLocations(map, validSpawnLocations);
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

        return validSpawnLocations;
    }

    private List<Vector3> GetValidSpawnLocations(int[,] map, List<Vector3> validSpawnLocations)
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

    private void PopulateMap(List<Vector3> validSpawnLocations)
    {
        Vector3 anchorPosition = _anchor.position;
        
        for (int locationIndex = 0; locationIndex < validSpawnLocations.Count; locationIndex++)
        {
            int randomObjectType = Random.Range(0, 100);
            Vector3 location = validSpawnLocations[locationIndex];

            float randomOffsetX = Random.Range(0, 1.5f);
            float randomOffsetZ = Random.Range(0, 1.5f);
            int randomRotation = Random.Range(0, 361);
            
            // Spawn trees at treeDensityPercent chance
            if (randomObjectType >= 0 && randomObjectType < treeDensityPercent)
            {
                int randomObject = Random.Range(0, treeOptionCount);

                _overworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(overworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + randomOffsetX, 0.5f, 
                        location.z * spacingOffset + anchorPosition.z + randomOffsetZ),
                    Quaternion.Euler(-90, randomRotation, 0));
                GameObject underworldDummyObject = Instantiate(underworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + Random.Range(0, 1.5f), -1.5f, 
                        location.z * spacingOffset + anchorPosition.z + Random.Range(0, 1.5f)),
                    Quaternion.Euler(90, randomRotation, 180));
                
                _underworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(underworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + randomOffsetX, -99.5f, 
                        location.z * spacingOffset + anchorPosition.z + randomOffsetZ),
                    Quaternion.Euler(-90, randomRotation, 0));
                GameObject overworldDummyObject = Instantiate(overworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + Random.Range(0, 1.5f), -101.5f, 
                        location.z * spacingOffset + anchorPosition.z + Random.Range(0, 1.5f)),
                    Quaternion.Euler(90, randomRotation, 180));
                    
                _overworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
                _underworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
                underworldDummyObject.transform.parent = _anchor;
                overworldDummyObject.transform.parent = _anchor;
            }
            
            // Spawn plants at plantDensityPercent chance
            else if (randomObjectType >= treeDensityPercent && randomObjectType < treeDensityPercent + plantDensityPercent)
            {
                int randomObject = Random.Range(treeOptionCount, treeOptionCount + plantOptionCount);
                
                _overworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(overworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + randomOffsetX, 0.5f, 
                        location.z * spacingOffset + anchorPosition.z + randomOffsetZ),
                    Quaternion.Euler(-90, randomRotation, 0));
                GameObject underworldDummyObject = Instantiate(underworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + Random.Range(0, 1.5f), -1.5f, 
                        location.z * spacingOffset + anchorPosition.z + Random.Range(0, 1.5f)),
                    Quaternion.Euler(90, randomRotation, 180));
                
                _underworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(underworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + randomOffsetX, -99.5f, 
                        location.z * spacingOffset + anchorPosition.z + randomOffsetZ),
                    Quaternion.Euler(-90, randomRotation, 0));
                GameObject overworldDummyObject = Instantiate(overworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + Random.Range(0, 1.5f), -101.5f, 
                        location.z * spacingOffset + anchorPosition.z + Random.Range(0, 1.5f)),
                    Quaternion.Euler(90, randomRotation, 180));
                    
                _overworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
                _underworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
                underworldDummyObject.transform.parent = _anchor;
                overworldDummyObject.transform.parent = _anchor;
            }
            
            // Spawn rocks at rockDensityPercent chance
            else if (randomObjectType >= treeDensityPercent + plantDensityPercent && randomObjectType <
                     treeDensityPercent + plantDensityPercent + rockDensityPercent)
            {
                int randomObject = Random.Range(treeOptionCount + plantOptionCount, treeOptionCount + plantOptionCount
                    + rockOptionCount);

                _overworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(
                    overworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + randomOffsetX, 0.5f,
                        location.z * spacingOffset + anchorPosition.z + randomOffsetZ),
                    Quaternion.Euler(-90, randomRotation, 0));
                GameObject underworldDummyObject = Instantiate(underworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + Random.Range(0, 1.5f), -1.5f,
                        location.z * spacingOffset + anchorPosition.z + Random.Range(0, 1.5f)),
                    Quaternion.Euler(90, randomRotation, 180));

                _underworldEnvironmentObjects[(int)location.x, (int)location.z] = Instantiate(
                    underworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + randomOffsetX, -99.5f,
                        location.z * spacingOffset + anchorPosition.z + randomOffsetZ),
                    Quaternion.Euler(-90, randomRotation, 0));
                GameObject overworldDummyObject = Instantiate(overworldEnvironmentObjects[randomObject],
                    new Vector3(location.x * spacingOffset + anchorPosition.x + Random.Range(0, 1.5f), -101.5f,
                        location.z * spacingOffset + anchorPosition.z + Random.Range(0, 1.5f)),
                    Quaternion.Euler(90, randomRotation, 180));

                _overworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
                _underworldEnvironmentObjects[(int)location.x, (int)location.z].transform.parent = _anchor;
                underworldDummyObject.transform.parent = _anchor;
                overworldDummyObject.transform.parent = _anchor;
            }
        }
    }
}
