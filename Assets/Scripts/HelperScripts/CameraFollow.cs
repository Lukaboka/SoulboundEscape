using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 5f;
    
    public Vector3 offset;
    
    private Transform _currentTarget;

    private void Awake()
    {
        _currentTarget = target;
    }
    
    void LateUpdate()
    {
        Vector3 targetCameraPosition = _currentTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position * -1, targetCameraPosition, smoothing + Time.deltaTime);
    }

}
