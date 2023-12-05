using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField] private bool lookAtTheCamera = false;

    void Update()
    {
        if (lookAtTheCamera)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
