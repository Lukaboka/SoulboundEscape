using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] 
    private GameObject[] overworldElements;
    
    [SerializeField] 
    private GameObject[] underworldElements;

    [SerializeField] 
    private GameObject invisibleWall;

    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    [SerializeField] private int spacingOffset = 1;
    [SerializeField] private int smoothingFactor = 5;
    [SerializeField] private int fillPercent = 80;

    private GameObject[,] _overworldTileMap;
    private GameObject[,] _underworldTileMap;


    // Start is called before the first frame update
    void Awake()
    {

        int [,] map = GenerateCellularAutomata(width, height, fillPercent, true, spacingOffset);

        _overworldTileMap = new GameObject[width / spacingOffset, height / spacingOffset];
        _underworldTileMap = new GameObject[width / spacingOffset, height / spacingOffset];

        map = SmoothMooreCellularAutomata(map, true, smoothingFactor);
        
        RenderMap(map, _overworldTileMap, _underworldTileMap);
    }

    public void RenderMap(int[,] map, GameObject[,] overworldTileMap, GameObject[,] underworldTileMap)
    {
        Transform pivot = transform;
        Vector3 pivotPosition = pivot.position;
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            for (int z = 0; z < map.GetUpperBound(1); z++)
            {
                if (map[x, z] == 0)
                {
                    overworldTileMap[x, z] = Instantiate(overworldElements[Random.Range(0, overworldElements.Length - 1)],
                        new Vector3(x * spacingOffset + pivotPosition.x, 0, z * spacingOffset + pivotPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    overworldTileMap[x, z].transform.parent = pivot;
                    underworldTileMap[x, z] = Instantiate(underworldElements[Random.Range(0, underworldElements.Length - 1)],
                        new Vector3(x * spacingOffset + pivotPosition.x, -1, z * spacingOffset + pivotPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    underworldTileMap[x, z].transform.parent = pivot;
                }
                else
                {
                    overworldTileMap[x, z] = Instantiate(invisibleWall,  
                        new Vector3(x * spacingOffset + pivotPosition.x, 0, z * spacingOffset + pivotPosition.z),
                        Quaternion.Euler(0, 0, 0));
                    overworldTileMap[x, z].transform.parent = pivot;
                }
            }
        }
    }
    
    public static int[,] GenerateCellularAutomata(int width, int height, int fillPercent, bool edgesAreWalls, int spacingOffset)
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
    
    static int GetMooreSurroundingTiles(int[,] map, int x, int z, bool edgesAreWalls)
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
    
    public static int[,] SmoothMooreCellularAutomata(int[,] map, bool edgesAreWalls, int smoothCount)
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
    
}
