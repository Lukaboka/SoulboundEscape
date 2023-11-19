using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 _input;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        GetPlayerInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetPlayerInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
    }

    void Look()
    {
        if (_input != Vector3.zero)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            Vector3 skewedInput = rotationMatrix.MultiplyPoint3x4(_input);
            
            Vector3 position = _transform.position;
            Vector3 relative = (position + skewedInput) - position;
            Quaternion rotation = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
        rb.MovePosition(_transform.position + _transform.forward * (_input.normalized.magnitude * (speed * Time.deltaTime)));
    }
}
