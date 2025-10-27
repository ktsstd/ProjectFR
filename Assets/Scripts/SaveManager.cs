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

    public int LanguageIndex = 0; // 0: Korean, 1: English, 2 �Ϻ���� �ϵ��� �ض� ����

    public bool canUseEsc = true;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("BgmVolume"))
        {
            SoundManager.Instance.BgmVolume = PlayerPrefs.GetFloat("BgmVolume");
            BgmSlider.value = SoundManager.Instance.BgmVolume;
            BgmText.text = "" + Mathf.Floor(SoundManager.Instance.BgmVolume * 100);
            SoundManager.Instance.Bgm.volume = SoundManager.Instance.BgmVolume; // ����
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
            LanguageIndex = 1; // 0 �ѱ��� 1 ����
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
                LText[0].text = "�游���";
                LText[1].text = "Ʃ�丮��";
                LText[2].text = "ȯ�漳��";
                LText[3].text = "��������";

                LText[4].text = "�Ҹ�";
                LText[5].text = "����";
                LText[6].text = "����";
                LText[7].text = "���Ǻ���";
                LText[8].text = "ȿ����";
                LText[9].text = "���� ȿ����";
                LText[10].text = "�Ҹ�";
                LText[11].text = "����";
                LText[12].text = "����";

                LText[13].text = "1�� ��ų";
                LText[14].text = "2�� ��ų";
                LText[15].text = "3�� ��ų";
                LText[16].text = "�̵���";
                LText[17].text = "�յ���";
                LText[18].text = "ī�޶� �����ü";
                LText[19].text = "ȯ�漳��";

                LText[20].text = "�ѱ�12�� ���� 24�� �Է� �����մϴ�";
                LText[21].text = "�г����� �Է��ϼ���";

                LText[22].text = "����";
                LText[23].text = "�ʱ�ȭ";

                LText[24].text = "��� ������ �ʱ�ȭ�˴ϴ�";
                LText[25].text = "�ʱ�ȭ�� ������ �ǵ��� �� �����ϴ�.";
                LText[26].text = "��";
                LText[27].text = "�ƴϿ�";

                LText[28].text = "�������� ���� ��������� �ֽ��ϴ�";
                LText[29].text = "���� ������ ������ �����Ͻð���ϱ�?";
                LText[30].text = "��";
                LText[31].text = "�ƴϿ�";

                LText[32].text = "���� ����";
                LText[33].text = "���� ����";
                LText[34].text = "���� ������Ʈ";
                LText[35].text = "������";

                LText[36].text = "����";
                LText[37].text = "����";
                LText[38].text = "����";
                LText[39].text = "����";
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
                LText[0].text = "���ڸ�";
                LText[1].text = "���ڸ�";
                LText[2].text = "���ڸ�";
                LText[3].text = "���ڸ�";
                LText[4].text = "������";
                LText[5].text = "�غ�";
                LText[6].text = "����";
                LText[7].text = "���� ����";

                LText[8].text = "�Ҹ�";
                LText[9].text = "����";
                LText[10].text = "����";
                LText[11].text = "���Ǻ���";
                LText[12].text = "ȿ����";
                LText[13].text = "���� ȿ����";
                LText[14].text = "�Ҹ�";
                LText[15].text = "����";
                LText[16].text = "����";

                LText[17].text = "1�� ��ų";
                LText[18].text = "2�� ��ų";
                LText[19].text = "3�� ��ų";
                LText[20].text = "�̵���";
                LText[21].text = "�յ���";
                LText[22].text = "ī�޶� �����ü";
                LText[23].text = "ȯ�漳��";

                LText[24].text = "�ѱ�12�� ���� 24�� �Է� �����մϴ�";
                LText[25].text = "�г����� �Է��ϼ���";

                LText[26].text = "����";
                LText[27].text = "�ʱ�ȭ";

                LText[28].text = "��� ������ �ʱ�ȭ�˴ϴ�";
                LText[29].text = "�ʱ�ȭ�� ������ �ǵ��� �� �����ϴ�.";
                LText[30].text = "��";
                LText[31].text = "�ƴϿ�";

                LText[32].text = "�������� ���� ��������� �ֽ��ϴ�";
                LText[33].text = "���� ������ ������ �����Ͻð���ϱ�?";
                LText[34].text = "��";
                LText[35].text = "�ƴϿ�";

                LText[36].text = "���� �̸�";
                LText[37].text = "��弱��";
                LText[38].text = "�⺻���";
                LText[39].text = "���Ѹ��";
                LText[40].text = "���潺 ���";
                LText[41].text = "�⺻���: ���õ�";
                LText[42].text = "����";
                LText[43].text = "���";

                LText[44].text = "����";
                LText[45].text = "����";
                LText[46].text = "����";
                LText[47].text = "����";
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
                LText[0].text = "�Ҹ�";
                LText[1].text = "����";
                LText[2].text = "����";
                LText[3].text = "���Ǻ���";
                LText[4].text = "ȿ����";
                LText[5].text = "���� ȿ����";
                LText[6].text = "�Ҹ�";
                LText[7].text = "����";
                LText[8].text = "����";

                LText[9].text = "1�� ��ų";
                LText[10].text = "2�� ��ų";
                LText[11].text = "3�� ��ų";
                LText[12].text = "�̵���";
                LText[13].text = "�յ���";
                LText[14].text = "ī�޶� �����ü";
                LText[15].text = "ȯ�漳��";

                LText[16].text = "�ѱ�12�� ���� 24�� �Է� �����մϴ�";
                LText[17].text = "�г����� �Է��ϼ���";

                LText[18].text = "����";
                LText[19].text = "�ʱ�ȭ";

                LText[20].text = "��� ������ �ʱ�ȭ�˴ϴ�";
                LText[21].text = "�ʱ�ȭ�� ������ �ǵ��� �� �����ϴ�.";
                LText[22].text = "��";
                LText[23].text = "�ƴϿ�";

                LText[24].text = "�������� ���� ��������� �ֽ��ϴ�";
                LText[25].text = "���� ������ ������ �����Ͻð���ϱ�?";
                LText[26].text = "��";
                LText[27].text = "�ƴϿ�";

                LText[28].text = "����";
                LText[29].text = "����";
                LText[30].text = "����";
                LText[31].text = "����";
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
            LangaugeText.text = "�ѱ���";
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
            LangaugeText.text = "�ѱ���";
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
        string userId = userIF.text.Trim();  // �� �г��� �Է°�

        if (string.IsNullOrEmpty(userIF.text))
        {
            userId = $"USER_{Random.Range(1, 21):00}";
        }
        else
        {
            userId = userIF.text;
        }

        // Photon ��Ʈ��ũ�� �� �г��� ����
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
