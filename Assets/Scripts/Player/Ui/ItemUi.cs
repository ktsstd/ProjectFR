using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemUi : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

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
        moneyText.text = localplayer.money.ToString();
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
                // 공증 / 돈내기 추가하기 / 텍스트 수정하기
                if (atkUpStack >= 1 && atkUpStack <= 10)
                {
                    player.playerAtk += 10;
                    atkUpStack++;
                }
                else if (atkUpStack >= 11 && atkUpStack <= 20)
                {
                    player.playerAtk += 15;
                    atkUpStack++;
                }
                else if (atkUpStack >= 21 && atkUpStack <= 30)
                {
                    player.playerAtk += 25;
                    atkUpStack++;
                }
                else
                {
                    player.playerAtk += 30;
                    atkUpStack++;
                }
                break;
            case 1:
                // 채증
                if (hpUpStack >= 1 && hpUpStack <= 10)
                {
                    player.playerHp += 75;
                    player.playerMaxHp += 75;
                    hpUpStack++;
                }
                else if (hpUpStack >= 11 && hpUpStack <= 20)
                {
                    player.playerHp += 100;
                    player.playerMaxHp += 100;
                    hpUpStack++;
                }
                else if (hpUpStack >= 21 && hpUpStack <= 30)
                {
                    player.playerHp += 150;
                    player.playerMaxHp += 150;
                    hpUpStack++;
                }
                else
                {
                    player.playerHp += 175;
                    player.playerMaxHp += 175;
                    hpUpStack++;
                }
                break;
            case 2:
                // 쿨감
                if (ctUpStack == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cooltimesave[i] = player.skillsCoolTime[i] * 0.1f;
                        player.skillsCoolTime[i] = player.skillsCoolTime[i] - (player.skillsCoolTime[i] * 0.1f);
                    }
                    ctUpStack++;
                }
                else if (ctUpStack == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        player.skillsCoolTime[i] -= cooltimesave[i];
                    }
                    ctUpStack++;
                }
                else if (ctUpStack == 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        player.skillsCoolTime[i] -= cooltimesave[i];
                    }
                    cooltimeButton.SetActive(false);
                }
                break;
        }
    }

    public void UseItem(int _player,int _num)
    {
        if (ItemSlotObject[_num].transform.childCount != 0)
            ItemSlotObject[_num].transform.GetChild(0).GetComponent<PlayerItem>().UseItem(_player);
    }

    public void ShowStore()
    {
        if (storeObject.activeSelf)
            storeObject.SetActive(false);
        else
            storeObject.SetActive(true);
    }
}