using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer arrowTip;
    [SerializeField]
    private MeshRenderer arrowBody;
    [SerializeField]

    private Vector3 fixedPosition;
    
    public Transform potion;
    public Transform candles;
    public Transform keys;
    public Transform portal;

    private Vector3 _potionPosition;
    private Vector3 _candlesPosition;
    private Vector3 _keysPosition;
    private Vector3 _portalPosition;
    
    private Vector3 _currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        arrowTip.enabled = false;
        arrowBody.enabled = false;
        
        fixedPosition = transform.position;
        
        _potionPosition = potion.transform.position;
        _potionPosition = new Vector3(_potionPosition.x, fixedPosition.y, _potionPosition.z);
        _candlesPosition = candles.transform.position;
        _candlesPosition =  new Vector3(_candlesPosition.x, fixedPosition.y, _candlesPosition.z);
        _keysPosition = keys.transform.position;
        _keysPosition = new Vector3(_keysPosition.x,  fixedPosition.y, _keysPosition.z);;
        _portalPosition = portal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector2 currentXZPosition = new Vector2(currentPosition.x, currentPosition.z);

        
        float distanceToCurrentTarget = Vector3.Distance(currentPosition, _currentTarget);
        float potionDistance;
        float candlesDistance;
        float keysDistance = Mathf.Infinity;

        if (GameManager.Instance.PortalOpen)
        {
           _currentTarget = new Vector3(_portalPosition.x, fixedPosition.y, _portalPosition.z);
        }
        else
        {
            if (!GameManager.Instance.GotPotion)
            {
                Vector2 potionXZPosition = new Vector2(_potionPosition.x, _potionPosition.z);
                potionDistance = Vector2.Distance(currentXZPosition, potionXZPosition);
            }
            else
            {
                if (_currentTarget == _potionPosition)
                    _currentTarget = new Vector3(1000, 1000, 1000);
                potionDistance = Mathf.Infinity;
            }

            if (!GameManager.Instance.GotPotion && potionDistance < distanceToCurrentTarget)
            {
                _currentTarget = _potionPosition;
            }

            if (!GameManager.Instance.GotCandles)
            {
                Vector2 candlesXZPosition = new Vector2(_candlesPosition.x, _candlesPosition.z);
                candlesDistance = Vector2.Distance(currentXZPosition, candlesXZPosition);
            }
            else
            {
                if (_currentTarget == _candlesPosition)
                    _currentTarget = new Vector3(1000, 1000, 1000);
                candlesDistance = Mathf.Infinity;
            }

            if (!GameManager.Instance.GotCandles && candlesDistance < distanceToCurrentTarget)
            {
                _currentTarget = _candlesPosition;
            }

            if (!GameManager.Instance.GotKeys)
            {
                Vector2 keysXZPosition = new Vector2(_keysPosition.x, _keysPosition.z);
            
                keysDistance = Vector2.Distance(currentXZPosition, keysXZPosition);
            }
            else
            {
                if (_currentTarget == _keysPosition)
                    _currentTarget = new Vector3(1000, 1000, 1000);
                keysDistance = Mathf.Infinity;
            }

            if (!GameManager.Instance.GotKeys && keysDistance < distanceToCurrentTarget)
            {
                _currentTarget = _keysPosition;
            }
            
        }
        
        transform.LookAt(_currentTarget);
    }

    public void SetActive(bool state)
    {
        arrowTip.enabled = state;
        arrowBody.enabled = state;
    }
}
