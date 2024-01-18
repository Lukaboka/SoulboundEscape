using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private AudioSource sfxSpell;
    private AudioSource sfxButton;


    private static AudioManager _instance = null;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {

        //get audiosource from gameobject
        sfxSpell = this.GetComponents<AudioSource>()[0];
        sfxButton = this.GetComponents<AudioSource>()[1];
    }

    public void Spell()
    {
        sfxSpell.Play();
    }

    public void Button()
    {
        sfxButton.Play();
    }
}
