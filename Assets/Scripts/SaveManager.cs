using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI WarningText;
    [SerializeField] TextMeshProUGUI BgmText;
    [SerializeField] TextMeshProUGUI SfxText;
    [SerializeField] TextMeshProUGUI SfxMonsterText;
    [SerializeField] GameObject SettingUI;
    public Slider BgmSlider;
    public Slider SfxSlider;
    public Slider SfxMonsterSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BgmVolume"))
        {
            SoundManager.Instance.BgmVolume = PlayerPrefs.GetFloat("BgmVolume");
            BgmSlider.value = SoundManager.Instance.BgmVolume;
            BgmText.text = "" + Mathf.Floor(SoundManager.Instance.BgmVolume * 100);
        }
        else
        {
            PlayerPrefs.SetFloat("BgmVolume", SoundManager.Instance.BgmVolume);
        }
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            SoundManager.Instance.SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            SfxSlider.value = SoundManager.Instance.SfxVolume;
            SfxText.text = "" + Mathf.Floor(SoundManager.Instance.SfxVolume * 100);
        }
        else
        {
            PlayerPrefs.SetFloat("SfxVolume", SoundManager.Instance.SfxVolume);
        }
        if (PlayerPrefs.HasKey("MonsterSfxVolume"))
        {
            SoundManager.Instance.SfxMonsterVolume = PlayerPrefs.GetFloat("MonsterSfxVolume");
            SfxMonsterSlider.value = SoundManager.Instance.SfxMonsterVolume;
        }
        else
        {
            PlayerPrefs.SetFloat("MonsterSfxVolume", SoundManager.Instance.SfxMonsterVolume);
        }
        BgmSlider.onValueChanged.AddListener(OnSliderBgmValueChanged);
        SfxSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
        SfxMonsterSlider.onValueChanged.AddListener(OnSliderSfxMonsterValueChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingUI.SetActive(!SettingUI.activeSelf);
            SoundManager.Instance.PlayUISfxShot(0);
        }
    }
    private void OnSliderBgmValueChanged(float newValue)
    {
        SoundManager.Instance.BgmVolume = newValue;
        BgmText.text = "" + Mathf.Floor(SoundManager.Instance.BgmVolume * 100);
        SoundManager.Instance.Bgm.volume = SoundManager.Instance.BgmVolume;
    }
    private void OnSliderSfxValueChanged(float newValue)
    {
        SoundManager.Instance.SfxVolume = newValue;
        SfxText.text = "" + Mathf.Floor(SoundManager.Instance.SfxVolume * 100);        
    }
    private void OnSliderSfxMonsterValueChanged(float newValue)
    {
        SoundManager.Instance.SfxMonsterVolume = newValue;
        SfxMonsterText.text = "" + Mathf.Floor(SoundManager.Instance.SfxMonsterVolume * 100);
    }
    public void OnClickSaveSetting()
    {
        PlayerPrefs.SetFloat("BgmVolume", SoundManager.Instance.BgmVolume);
        PlayerPrefs.SetFloat("SfxVolume", SoundManager.Instance.SfxVolume);
        PlayerPrefs.SetFloat("MonsterSfxVolume", SoundManager.Instance.SfxMonsterVolume);
        PlayerPrefs.Save();
    }
}
