using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Image crossPotion;
    [SerializeField] private Image crossKeys;
    [SerializeField] private Image crossCandles;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject info;

    // Start is called before the first frame update
    void Start()
    {
        settings.SetActive(false);
        info.SetActive(false);
        if (SceneManager.GetActiveScene().name == "MapGenerationScene")
        {
            crossPotion.enabled = false;
            crossKeys.enabled = false;
            crossCandles.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !info.activeSelf)
        {
            AudioManager.instance.Button();
            info.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.I) && info.activeSelf) 
        {
            CloseInfo();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !settings.activeSelf)
        {
            AudioManager.instance.Button();
            settings.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && settings.activeSelf)
        {
            CloseSettings();
        }
    }

    public void CloseSettings()
    {
        AudioManager.instance.Button();
        settings.SetActive(false);
    }

    public void CloseInfo()
    {
        AudioManager.instance.Button();
        info.SetActive(false);
    }

    public void PotionCrossOut()
    {
        crossPotion.enabled = true;
    }
    public void KeysCrossOut()
    {
        crossKeys.enabled = true;
    }
    public void CandlesCrossOut()
    {
        crossCandles.enabled = true;
    }
}
