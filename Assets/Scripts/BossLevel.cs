using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] int kill_count;

    public void BossKilled()
    {
        kill_count++;

        if(kill_count == 2)
        {
            GameManager.Instance.Escaped = true;
        }
    }
}
