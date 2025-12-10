using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUi : MonoBehaviour
{
    int[] itemPrice = new int[6] { 500, 200, 600, 800, 1200, 1000 };

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI[] statText;
    public TextMeshProUGUI[] statButtonText;

    public GameObject storeObject;
    public GameObject[] ItemSlotObject;
    public GameObject[] Items;
    public GameObject cooltimeButton;
    public TextMeshProUGUI[] ItemInfoTexts; // 0 : name, 1 : price, 2 : info
    public Image ItemImage;
    public Sprite[] ItemIcon;

    public Image FusionLockImage;
    public GameObject FusionLockButton;

    public PhotonView pv;
    public GameObject meteorOBJ;

    PlayerController localplayer;

    private void Start()
    {
        localplayer = GameManager.Instance.localPlayerCharacter.GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();

        if (GameManager.Instance.selectedMode == 1)
            FusionLockImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Instance.selectedMode == 1)
        {
            moneyText.text = localplayer.money.ToString();
            StateTextUpdate();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowStore();
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

    int setItem = 99;
    public void ShowItemInfo(int _num)
    {
        setItem = _num;
        switch (setItem) // item info text
        {
            case 0:
                ItemImage.sprite = ItemIcon[setItem];
                if (PlayerPrefs.GetInt("Language") == 0)
                {
                    ItemInfoTexts[0].text = "둔화 스크롤";
                    ItemInfoTexts[1].text = "500";
                    ItemInfoTexts[2].text = "5초동안 모든 몬스터의 이동속도를 감소시킵니다.";
                }
                else if (PlayerPrefs.GetInt("Language") == 1)
                {
                    ItemInfoTexts[0].text = "SlowScroll";
                    ItemInfoTexts[1].text = "500";
                    ItemInfoTexts[2].text = "Reduces the movement speed of all monsters for 5 seconds.";
                }
                break;
            case 1:
                ItemImage.sprite = ItemIcon[setItem];
                if (PlayerPrefs.GetInt("Language") == 0)
                {
                    ItemInfoTexts[0].text = "목책 소환";
                    ItemInfoTexts[1].text = "200";
                    ItemInfoTexts[2].text = "1분동안 유지되는 적의 이동 경로를 막는 구조물을 소환합니다. 3개까지 보유 가능합니다";
                }
                else if (PlayerPrefs.GetInt("Language") == 1)
                {
                    ItemInfoTexts[0].text = "Summon Fence";
                    ItemInfoTexts[1].text = "200";
                    ItemInfoTexts[2].text = "Summons a structure that blocks enemy movement for 1 minute. You can have up to 3 of these.";
                }
                break;
            case 2:
                ItemImage.sprite = ItemIcon[setItem];
                if (PlayerPrefs.GetInt("Language") == 0)
                {
                    ItemInfoTexts[0].text = "강화 스크롤";
                    ItemInfoTexts[1].text = "600";
                    ItemInfoTexts[2].text = "5초동안 사용자의 공격력을 35%증가시킵니다.";
                }
                else if (PlayerPrefs.GetInt("Language") == 1)
                {
                    ItemInfoTexts[0].text = "Atk Up Scroll";
                    ItemInfoTexts[1].text = "600";
                    ItemInfoTexts[2].text = "Increases the user's attack power by 35% for 5 seconds.";
                }
                break;
            case 3:
                ItemImage.sprite = ItemIcon[setItem];
                if (PlayerPrefs.GetInt("Language") == 0)
                {
                    ItemInfoTexts[0].text = "시간정지 스크롤";
                    ItemInfoTexts[1].text = "800";
                    ItemInfoTexts[2].text = "2초동안 행동 불가 상태가 되지만 피해를 받지 않습니다";
                }
                else if (PlayerPrefs.GetInt("Language") == 1)
                {
                    ItemInfoTexts[0].text = "Atk Up Scroll";
                    ItemInfoTexts[1].text = "800";
                    ItemInfoTexts[2].text = "You will be incapacitated for 2 seconds, but will not take damage.";
                }
                break;
            case 4:
                ItemImage.sprite = ItemIcon[setItem];
                if (PlayerPrefs.GetInt("Language") == 0)
                {
                    ItemInfoTexts[0].text = "화력 지원 스크롤";
                    ItemInfoTexts[1].text = "1200";
                    ItemInfoTexts[2].text = "모든 몬스터에게 최대 체력의 50%의 피해를 입힙니다(보스10%)";
                }
                else if (PlayerPrefs.GetInt("Language") == 1)
                {
                    ItemInfoTexts[0].text = "Atk Up Scroll";
                    ItemInfoTexts[1].text = "1200";
                    ItemInfoTexts[2].text = "Deals 50% of maximum health to all monsters (10% to bosses)";
                }
                break;
            case 5:
                ItemImage.sprite = ItemIcon[setItem];
                if (PlayerPrefs.GetInt("Language") == 0)
                {
                    ItemInfoTexts[0].text = "길드의 비호";
                    ItemInfoTexts[1].text = "1000";
                    ItemInfoTexts[2].text = "중앙 오브젝트의 HP를 10% 회복시킵니다.";
                }
                else if (PlayerPrefs.GetInt("Language") == 1)
                {
                    ItemInfoTexts[0].text = "Guild protection";
                    ItemInfoTexts[1].text = "1000";
                    ItemInfoTexts[2].text = "Restores 10% of the central object's HP";
                }
                break;
        }
    }

    public void AddItem()
    {
        if (setItem == 99) return;

        if (ItemSlotObject[0].transform.childCount == 1 &&
            ItemSlotObject[1].transform.childCount == 1 &&
            ItemSlotObject[2].transform.childCount == 1 &&
            ItemSlotObject[3].transform.childCount == 1) return;

        if (localplayer.money >= itemPrice[setItem])
        {
            localplayer.money -= itemPrice[setItem];

            if (setItem == 1) // 목책일경우 아이템 확인 후+1 아니면 넘어가기
            {
                GameObject[] items = HaveItem();
                foreach (GameObject item in items)
                {
                    if (item == null)
                        continue;

                    PlayerItem iteminfo = item.GetComponent<PlayerItem>();
                    if (iteminfo.iteminfo[0] == 1)
                        if (iteminfo.iteminfo[1] < 3)
                        {
                            iteminfo.iteminfo[1]++;
                            iteminfo.ShowUseItem();
                            return;
                        }
                }
            }

            for (int i = 0; i < ItemSlotObject.Length; i++) // 실질적 아이템 추가
            {
                if (ItemSlotObject[i].transform.childCount == 0)
                {
                    GameObject itme = Instantiate(Items[setItem]);
                    itme.transform.parent = ItemSlotObject[i].transform;
                    itme.transform.position = ItemSlotObject[i].transform.position;
                    itme.GetComponent<PlayerItem>().SetOnDragParent();
                    return;
                }
            }
        }
        else
        {
            // 돈부족 알림 띄우기
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

    public void BuyFusionSkill()
    {
        if (localplayer.money >= 1000)
        {
            localplayer.money -= 1000;
            localplayer.elementalkey = true;
            FusionLockImage.gameObject.SetActive(false);
            FusionLockButton.SetActive(false);
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

    [PunRPC]
    public void SkillEffect(Vector3 _pos)
    {
        Instantiate(meteorOBJ, _pos, meteorOBJ.transform.rotation);
        SoundManager.Instance.PlayItemSfx(3, _pos);
    }
}