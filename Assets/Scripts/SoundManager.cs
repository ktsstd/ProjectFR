using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public float BgmVolume = 1f;
    public float SfxVolume = 1f;
    [SerializeField] private AudioClip[] SfxAudio;
    [SerializeField] private AudioClip[] BgmAudio;
    [SerializeField] private AudioClip[] UISfxAudio;
    [SerializeField] private AudioSource Bgm;
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
        if (_instance != null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

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
    }
    public void PlaySfx(int index, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(SfxAudio[index], position, SfxVolume);
    }
    public void PlayUISfx(int index, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(UISfxAudio[index], position, SfxVolume);
    }
}
