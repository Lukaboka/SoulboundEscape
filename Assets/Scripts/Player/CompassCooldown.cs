using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassCooldown : MonoBehaviour
{

    [SerializeField] private PlayerController _playerController;

    private int _cooldown;
    private Image _cooldownCircle;

    private bool _coolingDown;
    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _cooldown = _playerController.cooldown;
        _cooldownCircle = GetComponent<Image>();
        _cooldownCircle.enabled = false;
        _coolingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_coolingDown)
        {
            _timer += Time.deltaTime;
            _cooldownCircle.fillAmount = 1 - _timer / _cooldown;
            if (_timer > _cooldown)
            {
                _cooldownCircle.enabled = false;
                _coolingDown = false;
            }
        }
    }

    public void StartCooldown()
    {
        _coolingDown = true;
        _cooldownCircle.enabled = true;
        _cooldownCircle.fillAmount = 1;
        _timer = 0;
    }
}
