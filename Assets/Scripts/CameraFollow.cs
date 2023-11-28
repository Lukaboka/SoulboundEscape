using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 5f;
    
    private Vector3 _offset;
    private Transform _currentTarget;

    private void Awake()
    {
        _currentTarget = target;
        _offset = transform.position - _currentTarget.position;
    }
    
    void LateUpdate()
    {
        Vector3 targetCameraPosition = _currentTarget.position + _offset;
        transform.position = Vector3.Lerp(transform.position * -1, targetCameraPosition, smoothing + Time.deltaTime);
    }
}
