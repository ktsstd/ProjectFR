using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;

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
        else
        {
            Debug.LogError("유효하지 않은 플레이어 인덱스입니다.");
        }
    }

    public void OnClickCharacterSelectRightButton()
    {
        if (isReady)
        {
            Debug.Log("WarningText 3 / alreadyReady");
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
            Debug.Log("WarningText 3 / alreadyReady");
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

    public void OnClickReadybutton()
    {
        if (CharacterIndex == -1)
        {
            Debug.Log("WarningText 1 / No Character");
            return;
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                continue;

            if (player.CustomProperties.TryGetValue("selectedCharacter", out object selectedChar) &&
                selectedChar is int selectedCharIndex &&
                selectedCharIndex == CharacterIndex)
            {
                Debug.Log("WarningText 2 / Can't Select");
                return;
            }
        }

        isReady = !isReady;

        Hashtable properties = new Hashtable
        {
            { "isReady", isReady },
            { "selectedCharacter", CharacterIndex }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }



    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
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
        else
        {
            Debug.LogError("유효하지 않은 플레이어 인덱스입니다.");
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