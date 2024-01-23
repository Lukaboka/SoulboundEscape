using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isBossScene = false;
    private static GameManager _instanceGameManager;

    public bool GotCandles { get; set; }
    public bool GotKeys { get; set; }
    public bool GotPotion { get; set; }
    
    public bool PortalOpen { get; set; }
    public bool Escaped { get; set; }
    

    public static GameManager Instance
    {
        get
        {
            if (_instanceGameManager is null)
                Debug.LogError("Game Manager is not intialized");
            
            return _instanceGameManager;
        }
        
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        _instanceGameManager = this;
        GotCandles = false;
        GotKeys = false;
        GotPotion = false;
        Escaped = false;
    }

    void Update()
    {
        if(isBossScene)
        {
            if(Escaped)
            {
                SceneManager.LoadScene("Win");
            }
        }

        if (GotCandles && GotKeys && GotPotion)
        {
            PortalOpen = true;
        }
        if(Escaped && !isBossScene)
        {
            SceneManager.LoadScene("BossScene");
        }
    }

    public void Lose()
    {
        StartCoroutine(dead());
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
