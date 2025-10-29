using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;

public class PlayerUi : MonoBehaviour
{
    public Slider playerHpSlider;
    public Image playerHpImage;
    public Slider playerDashSlider;

    public Sprite[] hpSprits;

    public Image playerIcon;
    public Sprite[] playerIconSprite;
    public Sprite[] otherPlayerIconSprite;
    public TextMeshProUGUI[] tooltipText;
    public Image[] skillsIcon;
    public Sprite[] fireSkillIcon;
    public Sprite[] waterSkillIcon;
    public Sprite[] lightningSkillIcon;
    public Sprite[] earthSkillIcon;

    public Image[] playerElementalSet;
    public Sprite[] elementalCodeSprite;
    public Sprite[] elementalCodeGraySprite;
    public Image fusionSkillIconImage;
    public Image fusionSkillGlassImage;
    public Sprite[] fusionSkillIcon;
    public Sprite[] fusionSkillGlassSprite;
    public Slider FusionHoldSlider;

    public GameObject[] otherPlayerUi;
    public Slider[] otherPlayerHp;
    public Image[] otherPlayerIcon;
    List<GameObject> otherPlayerList = new List<GameObject>();

    float playerHp;
    float playerMaxHp;

    float playerDash;
    float playerMaxDash;

    public TextMeshProUGUI[] skillCoolTimeText;
    float[] currntskillCoolTime = new float[3];
    float[] MaxskillCoolTime = new float[3];

    int myCode = 10;

    public float FusionHoldTime = 2;

    public Image[] skillCoolTime;

    void Start()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        playerHp = 1;
        playerMaxHp = 1;
        if ((int)character == 0)
        {
            playerIcon.sprite = playerIconSprite[0];
            playerHpImage.sprite = hpSprits[0];
            skillsIcon[0].sprite = waterSkillIcon[0];
            skillsIcon[1].sprite = waterSkillIcon[1];
            skillsIcon[2].sprite = waterSkillIcon[2];
            myCode = 0;
            tooltipText[0].text = TooltipTextSetting(myCode, 0);
            tooltipText[1].text = TooltipTextSetting(myCode, 1);
            tooltipText[2].text = TooltipTextSetting(myCode, 2);
        }
        else if ((int)character == 1)
        {
            playerIcon.sprite = playerIconSprite[1];
            playerHpImage.sprite = hpSprits[1];
            skillsIcon[0].sprite = lightningSkillIcon[0];
            skillsIcon[1].sprite = lightningSkillIcon[1];
            skillsIcon[2].sprite = lightningSkillIcon[2];
            myCode = 1;
            tooltipText[0].text = TooltipTextSetting(myCode, 0);
            tooltipText[1].text = TooltipTextSetting(myCode, 1);
            tooltipText[2].text = TooltipTextSetting(myCode, 2);
        }
        else if ((int)character == 2)
        {
            playerIcon.sprite = playerIconSprite[2];
            playerHpImage.sprite = hpSprits[1];
            skillsIcon[0].sprite = earthSkillIcon[0];
            skillsIcon[1].sprite = earthSkillIcon[1];
            skillsIcon[2].sprite = earthSkillIcon[2];
            myCode = 2;
            tooltipText[0].text = TooltipTextSetting(myCode, 0);
            tooltipText[1].text = TooltipTextSetting(myCode, 1);
            tooltipText[2].text = TooltipTextSetting(myCode, 2);
        }
        else if ((int)character == 3)
        {
            playerIcon.sprite = playerIconSprite[3];
            playerHpImage.sprite = hpSprits[3];
            skillsIcon[0].sprite = fireSkillIcon[0];
            skillsIcon[1].sprite = fireSkillIcon[1];
            skillsIcon[2].sprite = fireSkillIcon[2];
            myCode = 3;
            tooltipText[0].text = TooltipTextSetting(myCode, 0);
            tooltipText[1].text = TooltipTextSetting(myCode, 1);
            tooltipText[2].text = TooltipTextSetting(myCode, 2);
        }
    }

    void Update()
    {
        playerHpSlider.value = playerHp / playerMaxHp;
        playerDashSlider.value = 1 - (playerDash / playerMaxDash);

        FusionHoldSlider.value = 1 - (FusionHoldTime / 2f);

        for (int i = 0; i < 3; i++)
        {
            skillCoolTime[i].fillAmount = currntskillCoolTime[i] / MaxskillCoolTime[i];

            skillCoolTimeText[i].text = ((int)(currntskillCoolTime[i])).ToString();

            if (skillCoolTimeText[i].text == "0")
                skillCoolTimeText[i].gameObject.SetActive(false);
            else
                skillCoolTimeText[i].gameObject.SetActive(true);
        }

        if (otherPlayerList.Count != 0)
        {
            for (int i = 0; i < otherPlayerList.Count; i++)
            {
                otherPlayerHp[i].value = otherPlayerList[i].GetComponent<PlayerController>().playerHp / otherPlayerList[i].GetComponent<PlayerController>().playerMaxHp;
                if (otherPlayerIcon[i].sprite == null)
                    otherPlayerIcon[i].sprite = otherPlayerIconSprite[otherPlayerList[i].GetComponent<PlayerController>().elementalCode];
            }
        }
    }

    public void OtherPlayerData(GameObject _gameObject)
    {
        otherPlayerList.Add(_gameObject);

        for (int i = 0; i < otherPlayerList.Count; i++)
        {
            otherPlayerUi[i].SetActive(true);
        }
    }

    public void InputHpData(float _playerHp, float _playerMaxHp)
    {
        playerHp = _playerHp;
        playerMaxHp = _playerMaxHp;
    }

    public void InputDashData(float _playerDash, float _playerMaxDash)
    {
        playerDash = _playerDash;
        playerMaxDash = _playerMaxDash;
    }

    public void InputFusionSkillData(float _time, float _maxTime)
    {
        playerElementalSet[3].fillAmount = 1 - (_time / _maxTime);
    }

    public void GlassImageCrack(bool _bool)
    {
        if (!_bool)
            fusionSkillGlassImage.sprite = fusionSkillGlassSprite[0];
        else
            fusionSkillGlassImage.sprite = fusionSkillGlassSprite[1];
    }

    public void InputSkillData(float[] _skillCoolTime, float[] _skillMaxCoolTime)
    {
        currntskillCoolTime[0] = _skillCoolTime[0];
        currntskillCoolTime[1] = _skillCoolTime[1];
        currntskillCoolTime[2] = _skillCoolTime[2];

        MaxskillCoolTime[0] = _skillMaxCoolTime[0];
        MaxskillCoolTime[1] = _skillMaxCoolTime[1];
        MaxskillCoolTime[2] = _skillMaxCoolTime[2];
    }

    public void elementalData(int _code_1, int _code_2)
    {
        if (_code_1 == myCode || _code_2 == myCode)
            playerElementalSet[3].sprite = elementalCodeSprite[myCode];
        else
            playerElementalSet[3].sprite = elementalCodeGraySprite[myCode];

        for (int i = 0; i < otherPlayerList.Count; i++)
        {
            if (_code_1 == otherPlayerList[i].GetComponent<PlayerController>().elementalCode || _code_2 == otherPlayerList[i].GetComponent<PlayerController>().elementalCode)
            {
                playerElementalSet[i].sprite = elementalCodeSprite[otherPlayerList[i].GetComponent<PlayerController>().elementalCode];
            }
            else
            {
                playerElementalSet[i].sprite = elementalCodeGraySprite[otherPlayerList[i].GetComponent<PlayerController>().elementalCode];
            }
        }
    }
    string[,] tooltipTexts;
    public string TooltipTextSetting(int _code, int skillnum)
    {
        SaveManager SM = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            tooltipTexts = new string[4, 3] {
            { "���濡 �ĵ��� ������ ������ ���� �̵� �ӵ��� ���ҽ�Ű�� ���ظ� �����ϴ�", 
              "�ֺ� �Ʊ��� �ڽſ��� ��ȣ���� �ο��ϰ� ���ӽð� ���� �� ��ȣ���� ���� �ܷ���ŭ �÷��̾ ȸ����ŵ�ϴ�", 
              "������ ��ġ�� �߽����� ���� ������� ���ظ� ������ ������ �ҿ뵹�̸� ��ȯ�մϴ�" 
            },
            { "�ܰ��� �ֵѷ� ��ó�� ������ ���ظ� �����ϴ�", 
              "�������� ������ �����ϸ� ������ �ڸ��� �ִ� ������ ���ظ� �����ϴ�", 
              "�÷��̾ ��� ���� ���°� �Ǿ� ���� ���� ������ �������� Ÿ���մϴ�" },
            { "��ġ�� ������ ������ ������ ������ ���ظ� �ְ� ���ĳ��ϴ�", 
              "�ڽŰ� ��ȣ�ڿ��� ���� �ð� �����Ǵ� ��ȣ���� �ο��մϴ�", 
              "������ ��ġ�� ��ȣ�ڸ� ��ȯ�� ������� ���ظ� ������ ������ŵ�ϴ�. ���� ��ȣ�ڴ� ����� ���� �߰��� �����մϴ�." },
            { "ĳ������ ���濡 ���� �վ� �����մϴ�. \n �ش� ��ų�� ������Ͻ� �ٸ� ��ų�� ����Ҽ� �����ϴ�", 
              "������ ��ġ�� ����ź�� ���� ���������� ������ ���ظ� �����ϴ�.", 
              "���� �����ϸ� �����ϴ� ������ ȭ�� ��ǳ�� ��ȯ�մϴ� " }
            };
        }
        else if (PlayerPrefs.GetInt("Language") == 1)
        {
            tooltipTexts = new string[4, 3] {
            {
                "Unleashes a wave forward, dealing damage and slowing enemies it hits.",
                "Grants a shield to nearby allies and yourself. When the shield expires, heals for the remaining shield amount.",
                "Summons a powerful vortex at the target location, pulling in enemies and dealing damage."
            },
            {
                "Swings a dagger to damage nearby enemies.",
                "Dashes forward, damaging enemies in your path.",
                "Becomes invincible briefly and strikes enemies within range multiple times."
            },
            {
                "Slams the ground with a hammer, dealing damage and knocking enemies back.",
                "Applies a temporary shield to yourself and your guardian.",
                "Summons a guardian at the target location to slam the ground, dealing damage and stunning enemies. The guardian then pursues and attacks nearby enemies."
            },
            {
                "Breathes fire in front of the character. \nCannot use other skills while this skill is active.",
                "Throws a grenade at the target location, dealing continuous damage over time.",
                "Summons a powerful firestorm that tracks and attacks enemies."
            }
            };
        }
        return tooltipTexts[_code, skillnum];
    }
}
