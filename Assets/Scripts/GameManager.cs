using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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
        if (GotCandles && GotKeys && GotPotion)
        {
            PortalOpen = true;
        }
    }
}
