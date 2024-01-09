using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{

    [SerializeField] private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Update()
    {
        if (GameManager.Instance.GotPotion && GameManager.Instance.GotCandles && GameManager.Instance.GotKeys)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            var position1 = transform.position;
            Vector2 position = new Vector2(position1.x, position1.z);
        
            var position2 = player.transform.position;
            Vector2 playerPosition = new Vector2(position2.x, position2.z);

            if (Vector2.Distance(position, playerPosition) <= 0.5)
            {
                Debug.Log("Escaped!");
                GameManager.Instance.Escaped = true;
            }
        }
    }
}
