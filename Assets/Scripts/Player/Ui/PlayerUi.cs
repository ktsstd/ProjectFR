using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public Slider playerHpSlider;
    public Image playerHpImage;
    public Slider playerDashSlider;

    public Sprite[] hpSprits;

    public Image playerIcon;
    public Sprite[] playerIconSprite;
    public Sprite[] otherPlayerIconSprite;
    public Image[] skillsIcon;
    public Sprite[] fireSkillIcon;
    public Sprite[] waterSkillIcon;
    public Sprite[] lightningSkillIcon;
    public Sprite[] earthSkillIcon;


    public Image[] elementalCodeImage;
    public Sprite[] elementalCodeSprite;
    public Image fusionSkillIconImage;
    public Image fusionSkillLockImage;
    public Image fusionSkillGlassImage;
    public Sprite[] fusionSkillIcon;
    public Sprite[] fusionSkillGlassSprite;
    public Slider FusionHoldSlider;

    public GameObject[] otherPlayerUi;
    public Slider[] otherPlayerHp;
    public Image[] otherPlayerIcon;
    List <GameObject> otherPlayerList = new List<GameObject>();

    float playerHp;
    float playerMaxHp;

    float playerDash;
    float playerMaxDash;

    float[] currntskillCoolTime = new float[3];

    float[] MaxskillCoolTime = new float[3];

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
        }
        else if ((int)character == 1)
        {
            playerIcon.sprite = playerIconSprite[1];
            playerHpImage.sprite = hpSprits[1];
            skillsIcon[0].sprite = lightningSkillIcon[0];
            skillsIcon[1].sprite = lightningSkillIcon[1];
            skillsIcon[2].sprite = lightningSkillIcon[2];
        }
        else if ((int)character == 2)
        {
            playerIcon.sprite = playerIconSprite[2];
            playerHpImage.sprite = hpSprits[1];
            skillsIcon[0].sprite = earthSkillIcon[0];
            skillsIcon[1].sprite = earthSkillIcon[1];
            skillsIcon[2].sprite = earthSkillIcon[2];
        }
        else if ((int)character == 3)
        {
            playerIcon.sprite = playerIconSprite[3];
            playerHpImage.sprite = hpSprits[3];
            skillsIcon[0].sprite = fireSkillIcon[0];
            skillsIcon[1].sprite = fireSkillIcon[1];
            skillsIcon[2].sprite = fireSkillIcon[2];
        }
    }

    void Update()
    {
        playerHpSlider.value = playerHp / playerMaxHp;
        playerDashSlider.value = 1 - (playerDash / playerMaxDash);

        FusionHoldSlider.value = 1 - (FusionHoldTime / 2f);

        for (int i = 0;i < 3; i++)
        {
            skillCoolTime[i].fillAmount = currntskillCoolTime[i] / MaxskillCoolTime[i];
        }

        if (otherPlayerList.Count != 0)
        {
            for (int i = 0; i < otherPlayerList.Count; i++)
            {
                otherPlayerHp[i].value = otherPlayerList[i].GetComponent<PlayerController>().playerHp / otherPlayerList[i].GetComponent<PlayerController>().playerMaxHp;
                if (otherPlayerIcon[i].sprite == null)
                    otherPlayerIcon[i].sprite = playerIconSprite[otherPlayerList[i].GetComponent<PlayerController>().elementalCode];
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
        fusionSkillLockImage.fillAmount = _time / _maxTime;
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

        //if(_code_1 == 10)
        //    elementalCodeImage[0].sprite = elementalCodeSprite[4];
        //else
        //    elementalCodeImage[0].sprite = elementalCodeSprite[_code_1];

        //if (_code_2 == 10)
        //    elementalCodeImage[1].sprite = elementalCodeSprite[4];
        //else
        //    elementalCodeImage[1].sprite = elementalCodeSprite[_code_2];

        if ((_code_1 == 3 && _code_2 == 1) || (_code_1 == 1 && _code_2 == 3))
        {
            fusionSkillIconImage.sprite = fusionSkillIcon[0];
        }
        else if ((_code_1 == 0 && _code_2 == 1) || (_code_1 == 1 && _code_2 == 0))
        {
            fusionSkillIconImage.sprite = fusionSkillIcon[1];
        }
        else if ((_code_1 == 3 && _code_2 == 2) || (_code_1 == 2 && _code_2 == 3))
        {
            fusionSkillIconImage.sprite = fusionSkillIcon[2];
        }
        else if ((_code_1 == 0 && _code_2 == 2) || (_code_1 == 2 && _code_2 == 0))
        {
            fusionSkillIconImage.sprite = fusionSkillIcon[3];
        }
        else
            fusionSkillIconImage.sprite = elementalCodeSprite[4];
    }
}
