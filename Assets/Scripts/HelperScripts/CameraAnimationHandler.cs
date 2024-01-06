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
    
    [Header("Light Sources")] 
    [SerializeField] private Light overworldLight;
    [SerializeField] private Light underworldLight;

    private static readonly int InOverworld = Animator.StringToHash("inOverworld");

    // Start is called before the first frame update
    void Awake()
    {
        cameraOverworld.enabled = true;
        cameraUnderworld.enabled = false;
        overworldLight.enabled = true;
        underworldLight.enabled = false;
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
            overworldLight.enabled = false;
            underworldLight.enabled = true;

        }
        else
        {
            cameraOverworld.enabled = true;
            cameraUnderworld.enabled = false;
            overworldLight.enabled = true;
            underworldLight.enabled = false;
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

    public void InitializeCameras()
    {
        cameraOverworld.enabled = true;
        cameraUnderworld.enabled = false;
    }
}
