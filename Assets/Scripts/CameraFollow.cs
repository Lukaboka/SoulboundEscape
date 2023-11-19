using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetCameraPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing + Time.deltaTime);
    }
}
