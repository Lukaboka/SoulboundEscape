using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private bool overworldCamera;
    [SerializeField] private Animator fader;
    
    public Transform potion;
    public Transform candles;
    public Transform keys;
    
    public Vector3 offset;
    
    private Transform _currentTarget;
    private static readonly int FadingOut = Animator.StringToHash("FadingOut");

    private void Awake()
    {
        _currentTarget = target;
        if (overworldCamera)
        {
            StartCoroutine(InitialCameraPan());
        }
    }
    
    void LateUpdate()
    {
        Vector3 targetCameraPosition = _currentTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position * -1, targetCameraPosition, smoothing + Time.deltaTime);
    }

    IEnumerator InitialCameraPan()
    {
        _currentTarget = potion;
        yield return new WaitForSeconds(6);
        fader.SetBool(FadingOut, true);
        yield return new WaitForSeconds(1);
        fader.SetBool(FadingOut, false);
        _currentTarget = candles;
        yield return new WaitForSeconds(6);
        fader.SetBool(FadingOut, true);
        yield return new WaitForSeconds(1);
        fader.SetBool(FadingOut, false);
        _currentTarget = keys;
        yield return new WaitForSeconds(6);
        fader.SetBool(FadingOut, true);
        yield return new WaitForSeconds(1);
        fader.SetBool(FadingOut, false);
        _currentTarget = target;
        GameManager.Instance.DisableControls = false;
    }

}
