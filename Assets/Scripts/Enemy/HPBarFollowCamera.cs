using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarFollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 targetDirection;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + targetDirection);
    }
}
