using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource sfxHeal;
    private AudioSource sfxDash;
    private AudioSource sfxButton;
    private AudioSource sfxSpell;
    private AudioSource sfxButton2;
    private AudioSource sfxLightHit;
    private AudioSource sfxLightDeeperHit;
    private AudioSource sfxMuffleHit;
    private AudioSource sfxHardHit;
    private AudioSource sfxHeal2;
    private AudioSource sfxSlash;
    private AudioSource sfxSwordHit;
    private AudioSource sfxSwordHitBoss;
    private AudioSource sfxSwordHardHitBoss;
    private AudioSource sfxPickUp;
    private AudioSource sfxMechanicalButton;

    private AudioSource bgmIntro;
    private AudioSource bgmStory;
    private AudioSource bgmNormal;

    private AudioSource currentBGM;


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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        sfxHeal = this.GetComponents<AudioSource>()[0];
        sfxDash = this.GetComponents<AudioSource>()[1];
        sfxButton = this.GetComponents<AudioSource>()[2];
        sfxSpell = this.GetComponents<AudioSource>()[3];
        sfxButton2 = this.GetComponents<AudioSource>()[4];
        sfxLightHit = this.GetComponents<AudioSource>()[5];
        sfxLightDeeperHit = this.GetComponents<AudioSource>()[6];
        sfxMuffleHit = this.GetComponents<AudioSource>()[7];
        sfxHardHit = this.GetComponents<AudioSource>()[8];
        sfxHeal2 = this.GetComponents<AudioSource>()[9];
        sfxSlash = this.GetComponents<AudioSource>()[10];
        sfxSwordHit = this.GetComponents<AudioSource>()[11];
        sfxSwordHitBoss = this.GetComponents<AudioSource>()[12];
        sfxSwordHardHitBoss = this.GetComponents<AudioSource>()[13];
        sfxPickUp = this.GetComponents<AudioSource>()[14];
        sfxMechanicalButton = this.GetComponents<AudioSource>()[15];

        bgmIntro = this.GetComponents<AudioSource>()[16];
        bgmStory = this.GetComponents<AudioSource>()[17];
        bgmNormal = this.GetComponents<AudioSource>()[18];

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (currentBGM != null)
        {
            currentBGM.Stop();
        }

        if (scene.name.Equals("MainMenu"))
        {
            currentBGM = bgmIntro;
        }
        else
        {
            currentBGM = bgmNormal;
        }
        currentBGM.loop = true;
        currentBGM.Play();
    }

    public void StartStoryMusic()
    {
        currentBGM.Stop();
        currentBGM = bgmStory;
        currentBGM.loop = true;
        currentBGM.Play();
    }

    public void Heal()
    {
        sfxHeal.Play();
    }

    public void Dash()
    {
        sfxDash.Play();
    }

    public void Button()
    {
        sfxButton.Play();
    }

    public void Spell()
    {
        sfxSpell.Play();
    }

    public void Hit()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                sfxLightHit.Play();
                break;
            case 1:
                sfxLightDeeperHit.Play();
                break;
            case 2:
                sfxMuffleHit.Play();
                break;
        }
    }

    public void HardHit()
    {
        sfxHardHit.Play();
    }
    
    public void Heal2()
    {
        sfxHeal2.Play();
    }

    public void Slash()
    {
        sfxSlash.Play();
    }

    public void SwordHit() 
    { 
        sfxSwordHit.Play(); 
    }

    public void SwordHitBoss()
    {
        sfxSwordHitBoss.Play();
    }

    public void SwordHardHitBoss()
    {
        sfxSwordHardHitBoss.Play();
    }

    public void PickUp()
    {
        sfxPickUp.Play();
    }

    public void MechanicalButton()
    {
        sfxMechanicalButton.Play();
    }

}
