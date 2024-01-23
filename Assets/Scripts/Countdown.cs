using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] private Image countdown;
    [SerializeField] private float timeToSpawn;
    private int count = 0;
    
    private void Start()
    {
        countdown.fillAmount = 1f;
        StartCoroutine(CountdownMeter());
    }

    private IEnumerator CountdownMeter()
    {
        while(count < timeToSpawn)
        {
            yield return new WaitForSeconds(1f);
            countdown.fillAmount -= (float)(1f / timeToSpawn);
            count++;
        }
    }
}
