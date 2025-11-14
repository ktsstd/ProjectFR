using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [Header("UI Texts")]
    [SerializeField] TextMeshProUGUI[] LText;

    [Header("Settings UI Elements")]
    [SerializeField] TextMeshProUGUI WarningText;
    [SerializeField] TextMeshProUGUI BgmText;
    [SerializeField] TextMeshProUGUI SfxText;
    [SerializeField] TextMeshProUGUI SfxMonsterText;
    [SerializeField] GameObject SettingUI;
    [SerializeField] TMP_InputField userIF;
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI roomNameIF;
    [SerializeField] TextMeshProUGUI LangaugeText;

    public Slider BgmSlider;
    public Slider SfxSlider;
    public Slider SfxMonsterSlider;

    public int LanguageIndex = 0; // 0: Korean, 1: English, 2 일본어는 니들이 해라 수고

    public bool canUseEsc = true;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("BgmVolume"))
        {
            SoundManager.Instance.BgmVolume = PlayerPrefs.GetFloat("BgmVolume");
            BgmSlider.value = SoundManager.Instance.BgmVolume;
            BgmText.text = "" + Mathf.Floor(SoundManager.Instance.BgmVolume * 100);
            SoundManager.Instance.Bgm.volume = SoundManager.Instance.BgmVolume; // 몰라
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

        if (PlayerPrefs.HasKey("Language"))
        {
            LanguageIndex = PlayerPrefs.GetInt("Language");
        }
        else
        {
            LanguageIndex = 1; // 0 한국어 1 영어
            PlayerPrefs.SetInt("Language", LanguageIndex);
        }

            BgmSlider.onValueChanged.AddListener(OnSliderBgmValueChanged);
        SfxSlider.onValueChanged.AddListener(OnSliderSfxValueChanged);
        SfxMonsterSlider.onValueChanged.AddListener(OnSliderSfxMonsterValueChanged);
        if (roomNameIF != null)
            roomNameIF.text = PhotonNetwork.CurrentRoom.Name;
        if (userIF != null)
            userIF.text = PhotonNetwork.NickName;
        if (userName != null)
            userName.text = PhotonNetwork.NickName;
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        if (SceneManagerHelper.ActiveSceneName == "Main")
        {
            if (PlayerPrefs.GetInt("Language") == 0)
            {
                LText[0].text = "방만들기";
                LText[1].text = "튜토리얼";
                LText[2].text = "환경설정";
                LText[3].text = "게임종료";

                LText[4].text = "소리";
                LText[5].text = "조작";
                LText[6].text = "정보";
                LText[7].text = "음악볼륨";
                LText[8].text = "효과음";
                LText[9].text = "몬스터 효과음";
                LText[10].text = "소리";
                LText[11].text = "조작";
                LText[12].text = "정보";

                LText[13].text = "1번 스킬";
                LText[14].text = "2번 스킬";
                LText[15].text = "3번 스킬";
                LText[16].text = "이동기";
                LText[17].text = "합동기";
                LText[18].text = "카메라 잠금해체";
                LText[19].text = "환경설정";

                LText[20].text = "한글12자 영어 24자 입력 가능합니다";
                LText[21].text = "닉네임을 입력하세요";

                LText[22].text = "적용";
                LText[23].text = "초기화";

                LText[24].text = "모든 설정이 초기화됩니다";
                LText[25].text = "초기화한 설정은 되돌릴 수 없습니다.";
                LText[26].text = "예";
                LText[27].text = "아니요";

                LText[28].text = "저장하지 않은 변경사항이 있습니다";
                LText[29].text = "현재 적용한 설정을 저장하시곗습니까?";
                LText[30].text = "예";
                LText[31].text = "아니요";

                LText[32].text = "대기실 생성";
                LText[33].text = "대기실 입장";
                LText[34].text = "대기실 업데이트";
                LText[35].text = "나가기";

                LText[36].text = "변경";
                LText[37].text = "변경";
                LText[38].text = "변경";
                LText[39].text = "변경";
            }
            else if (PlayerPrefs.GetInt("Language") == 1)
            {
                LText[0].text = "MAKE ROOM";
                LText[1].text = "TUTORIAL";
                LText[2].text = "SETTING";
                LText[3].text = "QUIT";

                LText[4].text = "Sound";
                LText[5].text = "Keysetting";
                LText[6].text = "Info";
                LText[7].text = "Music";
                LText[8].text = "Sfx";
                LText[9].text = "Monster Sfx";
                LText[10].text = "Sound";
                LText[11].text = "Keysetting";
                LText[12].text = "Info";

                LText[13].text = "First Skill";
                LText[14].text = "Second Skill";
                LText[15].text = "Third Skill";
                LText[16].text = "Dash";
                LText[17].text = "Fusion Skill";
                LText[18].text = "Camera Unlock";
                LText[19].text = "Setting";

                LText[20].text = "Up to 24 English characters can be entered";
                LText[21].text = "Enter Nickname";

                LText[22].text = "Apply";
                LText[23].text = "Reset";

                LText[24].text = "All settings will be reset";
                LText[25].text = "Reset settings cannot be undone";
                LText[26].text = "Yes";
                LText[27].text = "No";

                LText[28].text = "You Have unsaved changes";
                LText[29].text = "Would you like to save the current settings";
                LText[30].text = "Yes";
                LText[31].text = "No";

                LText[32].text = "Make Waiting Room";
                LText[33].text = "Join Waiting Room";
                LText[34].text = "Update Waiting Room";
                LText[35].text = "Quit";

                LText[36].text = "Change";
                LText[37].text = "Change";
                LText[38].text = "Change";
                LText[39].text = "Change";
            }
        }
        else if (SceneManagerHelper.ActiveSceneName == "Lobby")
        {
            if (PlayerPrefs.GetInt("Language") == 0)
            {
                LText[0].text = "빈자리";
                LText[1].text = "빈자리";
                LText[2].text = "빈자리";
                LText[3].text = "빈자리";
                LText[4].text = "나가기";
                LText[5].text = "준비";
                LText[6].text = "시작";
                LText[7].text = "대기실 설정";

                LText[8].text = "소리";
                LText[9].text = "조작";
                LText[10].text = "정보";
                LText[11].text = "음악볼륨";
                LText[12].text = "효과음";
                LText[13].text = "몬스터 효과음";
                LText[14].text = "소리";
                LText[15].text = "조작";
                LText[16].text = "정보";

                LText[17].text = "1번 스킬";
                LText[18].text = "2번 스킬";
                LText[19].text = "3번 스킬";
                LText[20].text = "이동기";
                LText[21].text = "합동기";
                LText[22].text = "카메라 잠금해체";
                LText[23].text = "환경설정";

                LText[24].text = "한글12자 영어 24자 입력 가능합니다";
                LText[25].text = "닉네임을 입력하세요";

                LText[26].text = "적용";
                LText[27].text = "초기화";

                LText[28].text = "모든 설정이 초기화됩니다";
                LText[29].text = "초기화한 설정은 되돌릴 수 없습니다.";
                LText[30].text = "예";
                LText[31].text = "아니요";

                LText[32].text = "저장하지 않은 변경사항이 있습니다";
                LText[33].text = "현재 적용한 설정을 저장하시곗습니까?";
                LText[34].text = "예";
                LText[35].text = "아니요";

                LText[36].text = "대기실 이름";
                LText[37].text = "모드선택";
                LText[38].text = "기본모드";
                LText[39].text = "무한모드";
                LText[40].text = "오펜스 모드";
                LText[41].text = "기본모드: 선택됨";
                LText[42].text = "적용";
                LText[43].text = "취소";

                LText[44].text = "변경";
                LText[45].text = "변경";
                LText[46].text = "변경";
                LText[47].text = "변경";
            }
            else if (PlayerPrefs.GetInt("Language") == 1)
            {
                LText[0].text = "Empty Slot";
                LText[1].text = "Empty Slot";
                LText[2].text = "Empty Slot";
                LText[3].text = "Empty Slot";
                LText[4].text = "Quit";
                LText[5].text = "Ready";
                LText[6].text = "Start";
                LText[7].text = "Waiting Room Settings";

                LText[8].text = "Sound";
                LText[9].text = "Keysetting";
                LText[10].text = "Info";
                LText[11].text = "Music";
                LText[12].text = "Sfx";
                LText[13].text = "Monster Sfx";
                LText[14].text = "Sound";
                LText[15].text = "Keysetting";
                LText[16].text = "Info";

                LText[17].text = "First Skill";
                LText[18].text = "Second Skill";
                LText[19].text = "Third Skill";
                LText[20].text = "Dash";
                LText[21].text = "Fusion Skill";
                LText[22].text = "Camera Unlock";
                LText[23].text = "Setting";

                LText[24].text = "Up to 24 English characters or 12 Korean characters can be entered";
                LText[25].text = "Enter Nickname";

                LText[26].text = "Apply";
                LText[27].text = "Reset";

                LText[28].text = "All settings will be reset";
                LText[29].text = "Reset settings cannot be undone";
                LText[30].text = "Yes";
                LText[31].text = "No";

                LText[32].text = "You have unsaved changes";
                LText[33].text = "Would you like to save the current settings?";
                LText[34].text = "Yes";
                LText[35].text = "No";

                LText[36].text = "Waiting Room Name";
                LText[37].text = "Select Mode";
                LText[38].text = "Standard Mode";
                LText[39].text = "Endless Mode";
                LText[40].text = "Offense Mode";
                LText[41].text = "Standard Mode: Selected";
                LText[42].text = "Apply";
                LText[43].text = "Cancel";

                LText[44].text = "Change";
                LText[45].text = "Change";
                LText[46].text = "Change";
                LText[47].text = "Change";
            }
        }
        else if (SceneManagerHelper.ActiveSceneName == "Stage1" || SceneManagerHelper.ActiveSceneName == "Tutorial")
        {
            if (PlayerPrefs.GetInt("Language") == 0)
            {
                LText[0].text = "소리";
                LText[1].text = "조작";
                LText[2].text = "정보";
                LText[3].text = "음악볼륨";
                LText[4].text = "효과음";
                LText[5].text = "몬스터 효과음";
                LText[6].text = "소리";
                LText[7].text = "조작";
                LText[8].text = "정보";

                LText[9].text = "1번 스킬";
                LText[10].text = "2번 스킬";
                LText[11].text = "3번 스킬";
                LText[12].text = "이동기";
                LText[13].text = "합동기";
                LText[14].text = "카메라 잠금해체";
                LText[15].text = "환경설정";

                LText[16].text = "한글12자 영어 24자 입력 가능합니다";
                LText[17].text = "닉네임을 입력하세요";

                LText[18].text = "적용";
                LText[19].text = "초기화";

                LText[20].text = "모든 설정이 초기화됩니다";
                LText[21].text = "초기화한 설정은 되돌릴 수 없습니다.";
                LText[22].text = "예";
                LText[23].text = "아니요";

                LText[24].text = "저장하지 않은 변경사항이 있습니다";
                LText[25].text = "현재 적용한 설정을 저장하시곗습니까?";
                LText[26].text = "예";
                LText[27].text = "아니요";

                LText[28].text = "변경";
                LText[29].text = "변경";
                LText[30].text = "변경";
                LText[31].text = "변경";
            }
            else if (PlayerPrefs.GetInt("Language") == 1)
            {
                LText[0].text = "Sound";
                LText[1].text = "Keysetting";
                LText[2].text = "Info";
                LText[3].text = "Music";
                LText[4].text = "Sfx";
                LText[5].text = "Monster Sfx";
                LText[6].text = "Sound";
                LText[7].text = "Keysetting";
                LText[8].text = "Info";

                LText[9].text = "First Skill";
                LText[10].text = "Second Skill";
                LText[11].text = "Third Skill";
                LText[12].text = "Dash";
                LText[13].text = "Fusion Skill";
                LText[14].text = "Camera Unlock";
                LText[15].text = "Setting";

                LText[16].text = "Up to 24 English characters or 12 Korean characters can be entered";
                LText[17].text = "Enter Nickname";

                LText[18].text = "Apply";
                LText[19].text = "Reset";

                LText[20].text = "All settings will be reset";
                LText[21].text = "Reset settings cannot be undone";
                LText[22].text = "Yes";
                LText[23].text = "No";

                LText[24].text = "You have unsaved changes";
                LText[25].text = "Would you like to save the current settings?";
                LText[26].text = "Yes";
                LText[27].text = "No";

                LText[28].text = "Change";
                LText[29].text = "Change";
                LText[30].text = "Change";
                LText[31].text = "Change";
            }
        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            LangaugeText.text = "언어";
        }
        else if (PlayerPrefs.GetInt("Language") == 1)
        {
            LangaugeText.text = "Language";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canUseEsc)
        {
            SettingUI.SetActive(!SettingUI.activeSelf);
            SoundManager.Instance.PlayUISfxShot(0);
        }
    }

    public void LangaugeUpdate()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            LanguageIndex = 1;
            LangaugeText.text = "Language";
        }
        else if (PlayerPrefs.GetInt("Language") == 1)
        {
            LanguageIndex = 0;
            LangaugeText.text = "언어";
        }
        PlayerPrefs.SetInt("Language", LanguageIndex);
        UpdateLanguage();
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
