using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController: MonoBehaviour
{

    private InputHandler _input;
    
    [SerializeField] private Camera cameraOverworld;
    [SerializeField] private Camera cameraUnderworld;
    [SerializeField] private float movementSpeed;

    private Camera _activeCamera;
    private Vector3 _targetVector;
    private bool _swapped;
    
    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _activeCamera = cameraOverworld;
        cameraOverworld.enabled = true;
        cameraUnderworld.enabled = false;
        _swapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_swapped)
        {
            _targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        }
        else
        {
            _targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y * -1);
        }
        MoveTowardTarget(_targetVector); 
        RotateFromMouseVector();

        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            if (cameraOverworld.enabled)
            {
                cameraOverworld.enabled = false;
                cameraUnderworld.enabled = true;
            }
            else
            {
                cameraOverworld.enabled = true;
                cameraUnderworld.enabled = false;
            }

            _swapped = !_swapped;
        }

    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = movementSpeed * Time.deltaTime;
        targetVector = targetVector.normalized;
        targetVector = Quaternion.Euler(0, _activeCamera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var transform1 = transform;
        var targetPosition = transform1.position + targetVector * speed;
        transform1.position = targetPosition;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void RotateFromMouseVector()
    {
        Ray ray;
        if (_swapped)
        {
            ray = cameraUnderworld.ScreenPointToRay(_input.MousePosition);
        }
        else
        {
            ray = cameraOverworld.ScreenPointToRay(_input.MousePosition);
        }
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f)) return;
        var target = hitInfo.point;
        target.y = transform.position.y;
        transform.LookAt(target);
    }
}