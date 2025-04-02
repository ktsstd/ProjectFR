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

    public Image[] skillsIcon;
    public Sprite[] fireSkillIcon;
    public Sprite[] WaterSkillIcon;

    public Image[] elementalCodeImage;
    public Sprite[] elementalCodeSprite;

    float playerHp;
    float playerMaxHp;

    float playerDash;
    float playerMaxDash;

    float[] currntskillCoolTime = new float[3];

    float[] MaxskillCoolTime = new float[3];

    public Image[] skillCoolTime;

    void Start()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        playerHp = 1;
        playerMaxHp = 1;
        if ((int)character == 0)
        {
            playerHpImage.sprite = hpSprits[0];
            skillsIcon[0].sprite = WaterSkillIcon[0];
            skillsIcon[1].sprite = WaterSkillIcon[1];
            skillsIcon[2].sprite = WaterSkillIcon[2];
        }
        else if ((int)character == 3)
        {
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

        for (int i = 0;i < 3; i++)
        {
            skillCoolTime[i].fillAmount = currntskillCoolTime[i] / MaxskillCoolTime[i];
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
        if(_code_1 == 10)
            elementalCodeImage[0].sprite = null;
        else
            elementalCodeImage[0].sprite = elementalCodeSprite[_code_1];

        if (_code_2 == 10)
            elementalCodeImage[1].sprite = null;
        else
            elementalCodeImage[1].sprite = elementalCodeSprite[_code_2];
    }
}
