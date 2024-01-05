using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationHandler : MonoBehaviour
{

    [Header("Player Controller")] 
    [SerializeField] private PlayerController playerController;
    
    [Header("Player Cameras")]
    [SerializeField] private Camera cameraOverworld;
    [SerializeField] private Camera cameraUnderworld;

    [Header("Camera Animation Controller")] 
    [SerializeField] private Animator cameraController;

    private static readonly int InOverworld = Animator.StringToHash("inOverworld");

    // Start is called before the first frame update
    void Start()
    {
        cameraOverworld.enabled = true;
        cameraUnderworld.enabled = false;
    }

    public void SwapWorld()
    {
        if (cameraController.GetBool(InOverworld))
        {
            cameraController.SetBool(InOverworld, false);
        }
        else
        {
            cameraController.SetBool(InOverworld, true);
        }
    }

    public void SwapCamera()
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
    }

    public bool IsInAnimation()
    {
        if (cameraController.GetCurrentAnimatorStateInfo(0).IsName("CameraFadeToUnderworld") ||
            cameraController.GetCurrentAnimatorStateInfo(0).IsName("CameraFadeToOverworld"))
        {
            return true;
        }

        return false;
    }

    public void AdjustControls()
    {
        playerController.AdjustControls();
    }
}
