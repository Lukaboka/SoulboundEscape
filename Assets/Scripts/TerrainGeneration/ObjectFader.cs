using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectFader : MonoBehaviour
{

    [SerializeField] private float fadeSpeed = 10;
    [SerializeField] private float fadeAmount = 0.3f;
    

    private bool _opaque;
    private float _originalOpacity;
    private Renderer _renderer;
    private Material[] _mats;

    public bool doFade;
    public bool stayFaded;
    
    // Start is called before the first frame update
    void Start()
    {
        stayFaded = false;
        _mats = GetComponent<Renderer>().materials;
        for (int i = 0; i < _mats.Length; i++)
        {
            _originalOpacity = _mats[i].color.a;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doFade || stayFaded)
        {
            _opaque = false;
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }

    private void FadeOut()
    {
        for (int i = 0; i < _mats.Length; i++)
        {
            Color currentColor = _mats[i].color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, 
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            _mats[i].color = smoothColor;
        }
    }

    private void FadeIn()
    {
        for (int i = 0; i < _mats.Length; i++)
        {
            Color currentColor = _mats[i].color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, _originalOpacity, fadeSpeed * Time.deltaTime));
            _mats[i].color = smoothColor;
            
            if (currentColor.a >= 0.90f && !_opaque)
            {
                _opaque = true;
                transform.GetChild(0).GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
            }
        }
    }
}
