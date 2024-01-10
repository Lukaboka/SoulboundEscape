using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSurvivalMode : MonoBehaviour
{
    [SerializeField] private int numberOfEnemies;

    int currentNumberOfEnemies;

    private void Start()
    {
        currentNumberOfEnemies = numberOfEnemies;
    }

    public void EnemyDied()
    {
        currentNumberOfEnemies -= 1;

        if(currentNumberOfEnemies <= 0)
        {
            FindObjectOfType<GameManager_SurvivalMode>().WinLevel();
        }
    }
}
