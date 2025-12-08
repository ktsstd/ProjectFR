using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemUi : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI[] statText;
    public TextMeshProUGUI[] statButtonText;

    public GameObject storeObject;
    public GameObject[] ItemSlotObject;
    public GameObject[] Items;
    public GameObject cooltimeButton;

    PlayerController localplayer;

    private void Start()
    {
        localplayer = GameManager.Instance.localPlayerCharacter.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (GameManager.Instance.selectedMode == 1)
        {
            moneyText.text = localplayer.money.ToString();
            StateTextUpdate();
        }
    }

    public GameObject[] HaveItem()
    {
        GameObject[] items = new GameObject[ItemSlotObject.Length];

        for (int i = 0; i < ItemSlotObject.Length; i++)
        {
            if (ItemSlotObject[i].transform.childCount > 0)
                items[i] = ItemSlotObject[i].transform.GetChild(0).gameObject;
            else
                items[i] = null;
        }
        return items;
    }

    public void AddItem(int _num)
    {
        // n원 소비, 없으면 리턴 추가하기
        if (_num == 1)
        {
            GameObject[] items = HaveItem();
            foreach (GameObject item in items)
            {
                Debug.Log(item);
                if (item == null)
                    continue;

                PlayerItem iteminfo = item.GetComponent<PlayerItem>();
                if (iteminfo.iteminfo[0] == 1)
                    if (iteminfo.iteminfo[1] < 3)
                    {
                        iteminfo.iteminfo[1]++;
                        return;
                    }
            }
        }

        for (int i = 0; i < ItemSlotObject.Length; i++)
        {
            if (ItemSlotObject[i].transform.childCount == 0)
            {
                GameObject itme = Instantiate(Items[_num]);
                itme.transform.parent = ItemSlotObject[i].transform;
                itme.transform.position = ItemSlotObject[i].transform.position;
                itme.GetComponent<PlayerItem>().SetOnDragParent();
                return;
            }
        }
    }

    int atkUpStack = 1;
    int hpUpStack = 1;
    int ctUpStack = 1;
    float[] cooltimesave = new float[3] { 0, 0, 0 };

    public void BuyState(int _num)
    {
        PlayerController player = GameManager.Instance.localPlayerCharacter.GetComponent<PlayerController>();
        switch (_num)
        {
            case 0:
                if (atkUpStack >= 1 && atkUpStack <= 10)
                {
                    if (localplayer.money >= 300)
                    {
                        localplayer.money -= 300;
                        player.playerAtk += 10;
                        atkUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                else if (atkUpStack >= 11 && atkUpStack <= 20)
                {
                    if (localplayer.money >= 450)
                    {
                        localplayer.money -= 450;
                        player.playerAtk += 15;
                        atkUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                else if (atkUpStack >= 21 && atkUpStack <= 30)
                {
                    if (localplayer.money >= 500)
                    {
                        localplayer.money -= 500;
                        player.playerAtk += 25;
                        atkUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                else
                {
                    if (localplayer.money >= 500)
                    {
                        localplayer.money -= 500;
                        player.playerAtk += 30;
                        atkUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                break; // 공증
            case 1:
                if (hpUpStack >= 1 && hpUpStack <= 10)
                {
                    if (localplayer.money >= 300)
                    {
                        localplayer.money -= 300;
                        player.playerHp += 75;
                        player.playerMaxHp += 75;
                        hpUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                else if (hpUpStack >= 11 && hpUpStack <= 20)
                {
                    if (localplayer.money >= 450)
                    {
                        localplayer.money -= 450;
                        player.playerHp += 100;
                        player.playerMaxHp += 100;
                        hpUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                else if (hpUpStack >= 21 && hpUpStack <= 30)
                {
                    if (localplayer.money >= 500)
                    {
                        localplayer.money -= 500;
                        player.playerHp += 150;
                        player.playerMaxHp += 150;
                        hpUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                else
                {
                    if (localplayer.money >= 500)
                    {
                        localplayer.money -= 500;
                        player.playerHp += 175;
                        player.playerMaxHp += 175;
                        hpUpStack++;
                    }
                    else
                    { // 돈부족 UI 띄우기
                        return;
                    }
                }
                break; // 체증
            case 2:
                // 쿨감
                if (ctUpStack == 1)
                {
                    if (localplayer.money >= 1000)
                    {
                        localplayer.money -= 1000;
                        for (int i = 0; i < 3; i++)
                        {
                            cooltimesave[i] = player.skillsCoolTime[i] * 0.1f;
                            player.skillsCoolTime[i] = player.skillsCoolTime[i] - (player.skillsCoolTime[i] * 0.1f);
                        }
                        ctUpStack++;
                    }
                }
                else if (ctUpStack == 2)
                {
                    if (localplayer.money >= 1000)
                    {
                        localplayer.money -= 1000;
                        for (int i = 0; i < 3; i++)
                        {
                            player.skillsCoolTime[i] -= cooltimesave[i];
                        }
                        ctUpStack++;
                    }
                }
                else if (ctUpStack == 3)
                {

                    if (localplayer.money >= 1000)
                    {
                        localplayer.money -= 1000;
                        for (int i = 0; i < 3; i++)
                        {
                            player.skillsCoolTime[i] -= cooltimesave[i];
                        }
                        cooltimeButton.SetActive(false);
                    }
                }
                break; // 쿨감
        }
    }

    public void StateTextUpdate()
    {
        if (atkUpStack >= 1 && atkUpStack <= 10)
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[0].text = "공격력 증가\n+10";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[0].text = "ATK UP\n+10";

            statButtonText[0].text = "-300";
        }
        else if (atkUpStack >= 11 && atkUpStack <= 20)
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[0].text = "공격력 증가\n+15";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[0].text = "ATK UP\n+15";

            statButtonText[0].text = "-450";
        }
        else if (atkUpStack >= 21 && atkUpStack <= 30)
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[0].text = "공격력 증가\n+20";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[0].text = "ATK UP\n+20";

            statButtonText[0].text = "-500";
        }
        else
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[0].text = "공격력 증가\n+30";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[0].text = "ATK UP\n+30";

            statButtonText[0].text = "-500";
        }


        if (hpUpStack >= 1 && hpUpStack <= 10)
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[1].text = "생명력 증가\n+75";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[1].text = "MAXHP UP\n+75";

            statButtonText[1].text = "-300";
        }
        else if (hpUpStack >= 11 && hpUpStack <= 20)
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[1].text = "생명력 증가\n+100";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[1].text = "MAXHP UP\n+100";

            statButtonText[1].text = "-450";
        }
        else if (hpUpStack >= 21 && hpUpStack <= 30)
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[1].text = "생명력 증가\n+150";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[1].text = "MAXHP UP\n+150";

            statButtonText[1].text = "-500";
        }
        else
        {
            if (PlayerPrefs.GetInt("Language") == 0)
                statText[1].text = "생명력 증가\n+175";
            else if (PlayerPrefs.GetInt("Language") == 1)
                statText[1].text = "HP UP\n+175";

            statButtonText[1].text = "-500";
        }

        if (PlayerPrefs.GetInt("Language") == 0)
            statText[2].text = "쿨타임 감소\n10%";
        else if (PlayerPrefs.GetInt("Language") == 1)
            statText[2].text = "CoolDown\n10%";

    }

    public void UseItem(int _player,int _num, Vector3 _usePos)
    {
        if (ItemSlotObject[_num].transform.childCount != 0)
            ItemSlotObject[_num].transform.GetChild(0).GetComponent<PlayerItem>().UseItem(_player, _usePos);
    }

    public void ShowStore()
    {
        if (storeObject.activeSelf)
            storeObject.SetActive(false);
        else
            storeObject.SetActive(true);
    }
}