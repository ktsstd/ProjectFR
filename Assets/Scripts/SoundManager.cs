using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    [SerializeField] public float BgmVolume = 0.2f;
    [SerializeField] public float SfxVolume = 0.5f;
    [SerializeField] public float SfxMonsterVolume = 0.5f;
    //[SerializeField] private AudioClip[] SfxAudio;
    [SerializeField] private AudioClip[] SfxPlayerAudio;
    [SerializeField] private AudioClip[] SfxMonsterAudio;
    [SerializeField] private AudioClip[] BgmAudio;
    [SerializeField] private AudioClip[] UISfxAudio;
    [SerializeField] public AudioSource Bgm;
    [SerializeField] private AudioSource Sfx;
    [SerializeField] private AudioSource UISfx;
    [SerializeField] private AudioSource[] MonsterAudio;
    
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            }
            return _instance;
        }
    }
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("SfxVolume"))
        {
            PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        }
        else
        {
            SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        }

        if (!PlayerPrefs.HasKey("BgmVolume"))
        {
            PlayerPrefs.SetFloat("BgmVolume", BgmVolume);
        }
        else
        {
            BgmVolume = PlayerPrefs.GetFloat("BgmVolume");
        }

        if (!PlayerPrefs.HasKey("MonsterSfxVolume"))
        {
            PlayerPrefs.SetFloat("MonsterSfxVolume", SfxMonsterVolume);
        }
        else
        {
            SfxMonsterVolume = PlayerPrefs.GetFloat("MonsterSfxVolume");
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void PlayPlayerSfx(int index, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(SfxPlayerAudio[index], position, SfxVolume);
    }
    public void PlayMonsterSfx(int index, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(SfxMonsterAudio[index], position, SfxMonsterVolume);
    }
    public void OnClickUISfx()
    {
        PlayUISfxShot(0);
    }

    public void PlayUISfxShot(int index)
    {
        GameObject tempAudioObj = new GameObject("TempUISfxAudio");
        AudioSource audioSource = tempAudioObj.AddComponent<AudioSource>();

        audioSource.clip = UISfxAudio[index];
        audioSource.volume = SfxVolume * 5f;
        audioSource.spatialBlend = 0f; 
        audioSource.Play();

        Destroy(tempAudioObj, UISfxAudio[index].length);
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
            return;
        PlayBgm(scene.name);
    }

    private void PlayBgm(string sceneName)
    {
        Bgm.Stop();

        switch (sceneName)
        {
            case "Main":
                Bgm.clip = BgmAudio[0];
                break;
            case "Test":
                Bgm.clip = BgmAudio[1];
                break;
            case "Boss":
                Bgm.clip = BgmAudio[2];
                break;
            default:
                break;
        }

        Bgm.volume = BgmVolume;
        Bgm.loop = true;
        Bgm.Play();
    }

}
