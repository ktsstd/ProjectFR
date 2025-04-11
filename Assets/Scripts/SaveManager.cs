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
    [SerializeField] TMP_InputField userIF;
    [SerializeField] TextMeshProUGUI roomNameIF;
    public Slider BgmSlider;
    public Slider SfxSlider;
    public Slider SfxMonsterSlider;

    public bool canUseEsc = true;

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
            BgmSlider.value = SoundManager.Instance.BgmVolume;
            BgmText.text = "" + Mathf.Floor(SoundManager.Instance.BgmVolume * 100);
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
            SfxSlider.value = SoundManager.Instance.SfxVolume;
            SfxText.text = "" + Mathf.Floor(SoundManager.Instance.SfxVolume * 100);
        }
        if (PlayerPrefs.HasKey("MonsterSfxVolume"))
        {
            SoundManager.Instance.SfxMonsterVolume = PlayerPrefs.GetFloat("MonsterSfxVolume");
            SfxMonsterSlider.value = SoundManager.Instance.SfxMonsterVolume;
            SfxMonsterText.text = "" + Mathf.Floor(SoundManager.Instance.SfxMonsterVolume * 100);
        }
        else
        {
            PlayerPrefs.SetFloat("MonsterSfxVolume", SoundManager.Instance.SfxMonsterVolume);
            SfxMonsterSlider.value = SoundManager.Instance.SfxMonsterVolume;
            SfxMonsterText.text = "" + Mathf.Floor(SoundManager.Instance.SfxMonsterVolume * 100);
        }
        BgmSlider.onValueChanged.AddListener(OnSliderBgmValueChanged);
        SfxSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
        SfxMonsterSlider.onValueChanged.AddListener(OnSliderSfxMonsterValueChanged);
        if (roomNameIF != null)
            roomNameIF.text = PhotonNetwork.CurrentRoom.Name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canUseEsc)
        {
            SettingUI.SetActive(!SettingUI.activeSelf);
            SoundManager.Instance.PlayUISfxShot(0);
        }
    }
    public void OnClickUISfx()
    {
        SoundManager.Instance.PlayUISfxShot(0);
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
    
    public void OnChangeNickname()
    {
        string userId = userIF.text.Trim();  // 새 닉네임 입력값

        if (string.IsNullOrEmpty(userIF.text))
        {
            userId = $"USER_{Random.Range(1, 21):00}";
        }
        else
        {
            userId = userIF.text;
        }

        // Photon 네트워크에 새 닉네임 설정
        PhotonNetwork.NickName = userId;
        PlayerPrefs.SetString("UserId", userId);
    }
    public void OnClickSaveSetting()
    {
        PlayerPrefs.SetFloat("BgmVolume", SoundManager.Instance.BgmVolume);
        PlayerPrefs.SetFloat("SfxVolume", SoundManager.Instance.SfxVolume);
        PlayerPrefs.SetFloat("MonsterSfxVolume", SoundManager.Instance.SfxMonsterVolume);
        PlayerPrefs.Save();
    }

    public void OnClickResetSetting()
    {
        PlayerPrefs.DeleteAll();
        BgmSlider.value = 0.2f;
        SfxSlider.value = 0.5f;
        SfxMonsterSlider.value = 0.5f;
        BgmSlider.onValueChanged.AddListener(OnSliderBgmValueChanged);
        SfxSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
        SfxMonsterSlider.onValueChanged.AddListener(OnSliderSfxMonsterValueChanged);
    }
    public void OnClickResetSettingInGame()
    {
        PlayerPrefs.DeleteKey("BgmVolume");
        PlayerPrefs.DeleteKey("SfxVolume");
        PlayerPrefs.DeleteKey("MonsterSfxVolume");
        BgmSlider.value = 0.2f;
        SfxSlider.value = 0.5f;
        SfxMonsterSlider.value = 0.5f;
        BgmSlider.onValueChanged.AddListener(OnSliderBgmValueChanged);
        SfxSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
        SfxMonsterSlider.onValueChanged.AddListener(OnSliderSfxMonsterValueChanged);
    }
}
