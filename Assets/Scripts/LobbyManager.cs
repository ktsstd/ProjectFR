using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;
    public TMP_Text[] playerTexts;
    private List<Player> playerList = new List<Player>();
    public GameObject[] ReadyObj;
    public GameObject StartObj;
    private bool isReady;
    public int CharacterIndex;
    public GameObject[] CharacterImg;
    public GameObject CharacterImgParent;
    public Transform[] CharacterPos;
    public TextMeshProUGUI WarningText;

    //[SerializeField] GameObject SettingUI;

    private void Start()
    {
        CharacterIndex = -1;
        isReady = false;
        
        if (roomNameText != null)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
            bool playerIsReady = player.CustomProperties.TryGetValue("isReady", out object readyState) && (bool)readyState;

            SetReadyState(player.ActorNumber, playerIsReady);
        }

        UpdatePlayerListUI();
        int localPlayerIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        if(localPlayerIndex >= 0 && localPlayerIndex < CharacterPos.Length)
        {
            CharacterImgParent.transform.position = CharacterPos[localPlayerIndex].position;
        }
    }

    public void OnClickCharacterSelectRightButton()
    {
        if (isReady)
        {
            WarningTexts(3);
            return;
        }
        if (CharacterIndex < 3)
        {
            CharacterIndex += 1;
        }
        else
        {
            CharacterIndex = 0;
        }
        for (int i = 0; i <= 3; i++)
            {
                if (CharacterImg[i] != null)
                {
                    CharacterImg[i].SetActive(false);
                }
            }
            CharacterImg[CharacterIndex].SetActive(true);
    }

    public void OnClickCharacterSelectLeftButton()
    {
        if (isReady)
        {
            WarningTexts(3);
            return;
        }
        if (CharacterIndex > 0)
        {
            CharacterIndex -= 1;
        }
        else
        {
            CharacterIndex = 3;
        }
        for (int i = 0; i <= 3; i++)
            {
                if (CharacterImg[i] != null)
                {
                    CharacterImg[i].SetActive(false);
                }
            }
            CharacterImg[CharacterIndex].SetActive(true);
    }

    private GameObject CharacterObj;
    public void OnClickReadybutton()
    {
        if (CharacterIndex == -1)
        {
            WarningTexts(2);
            return;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                continue;

            if (player.CustomProperties.TryGetValue("selectedCharacter", out object selectedChar) &&
                selectedChar is int selectedCharIndex &&
                selectedCharIndex == CharacterIndex)
            {
                WarningTexts(1);
                return;
            }
        }

        isReady = !isReady;
        

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", isReady },
            { "selectedCharacter", CharacterIndex }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        if (isReady)
        {
            CharacterObj = PhotonNetwork.Instantiate("Lobby/Icon" + CharacterIndex, CharacterPos[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity, 0);
            CharacterObj.transform.SetParent(CharacterImgParent.transform, false);
            CharacterObj.transform.localPosition = CharacterPos[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
        }
        else
        {
            PhotonNetwork.Destroy(CharacterObj);
        }
    }

    private void WarningTexts(int ErrorCode)
    {
        if (isFadeOut) return;
        Color color = WarningText.color;
        color.a = 1f;
        WarningText.color = color;

        if (ErrorCode == 1)
            WarningText.text = "이미 선택된 캐릭터입니다.";
        else if (ErrorCode == 2)
            WarningText.text = "캐릭터를 선택해주세요.";
        else if (ErrorCode == 3)
            WarningText.text = "이미 준비 완료 상태입니다.";
        else
            WarningText.text = "알 수 없는 오류입니다.";

        StartCoroutine(FadeOutText());
    }

    bool isFadeOut = false;
    private IEnumerator FadeOutText()
    {
        isFadeOut = true;
        Color color = WarningText.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / 2f;
            WarningText.color = color;
            yield return null;
        }
        isFadeOut = false;
    }


    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("isReady"))
        {
            bool playerIsReady = (bool)changedProps["isReady"];
            SetReadyState(targetPlayer.ActorNumber, playerIsReady);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateStartButton", RpcTarget.All, isAllPlayerReady());
        }
    }


    [PunRPC]
    public void UpdateStartButton(bool state)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartObj.SetActive(state);
        }
    }

    public void OnClickGameStart()
    {
        PhotonNetwork.LoadLevel("Test");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);
        bool playerIsReady = newPlayer.CustomProperties.TryGetValue("isReady", out object readyState) && (bool)readyState;
        SetReadyState(newPlayer.ActorNumber, playerIsReady);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateStartButton", RpcTarget.All, false);
        }

        UpdatePlayerListUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int indexToRemove = otherPlayer.ActorNumber; // playerList.FindIndex(p => p.ActorNumber == otherPlayer.ActorNumber);
        
        if (indexToRemove != -1)
        {
            playerList.RemoveAt(indexToRemove);

            if (ReadyObj[indexToRemove] != null)
            {
                ReadyObj[indexToRemove].SetActive(false);
            }
        }

        int localPlayerIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        if(localPlayerIndex >= 0 && localPlayerIndex < CharacterPos.Length)
        {
            CharacterImgParent.transform.position = CharacterPos[localPlayerIndex].position;
        }

        SetReadyState(otherPlayer.ActorNumber, false);
        UpdatePlayerListUI();
    }

    private void SetReadyState(int playerActorNumber, bool isReady)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == playerActorNumber)
            {
                if (ReadyObj[i] != null)
                {
                    ReadyObj[i].SetActive(isReady);
                }
                break;
            }
        }
    }

    private bool isAllPlayerReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.TryGetValue("isReady", out object isReady) || !(bool)isReady)
            {
                return false;
            }
        }
        //if (PhotonNetwork.PlayerList.Length < 2)
        //{
        //    return false;
        //}
        return true;
    }

    void UpdatePlayerListUI()
    {
        for (int i = 0; i < playerTexts.Length; i++)
        {
            playerTexts[i].text = "";
        }

        for (int i = 0; i < playerList.Count && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerList[i].NickName;
        }
    }
}