using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeMenu : MonoBehaviour
{
    private AudioManager am;
    public Slider volumeMusic;
    public Slider volumeSfx;
    public Slider volumeMaster;
    
    void OnEnable()
    {
        am = AudioManager.instance;
        volumeMusic.maxValue = 1.0f;
        volumeSfx.maxValue = 1.0f;
        volumeMaster.maxValue = 1.0f;

        volumeMusic.value = AudioManager.instance.volumeMusic;
        volumeSfx.value = AudioManager.instance.volumeSfx;
        volumeMaster.value = AudioManager.instance.volumeMaster;
    }

    public void OnValueChange()
    {
        if(am == null)
            return;
        
        am.volumeMusic = volumeMusic.value;
        am.volumeSfx = volumeSfx.value;
        am.volumeMaster = volumeMaster.value;

        AudioManager.instance?.ChangeVolumeSettings();
    }

}
