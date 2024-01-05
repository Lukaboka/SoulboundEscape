using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudgetParallax : MonoBehaviour
{

    [SerializeField] 
    private Renderer bgRenderer;

    [SerializeField] 
    private float speed;
    

    // Update is called once per frame
    void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
