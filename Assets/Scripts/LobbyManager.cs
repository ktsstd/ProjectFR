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
    //private bool isWater;
    //private bool isThunder;
    //private bool isFire;
    //private bool isGround;

    private void Start()
    {
        isReady = false;

        //Hashtable properties = new Hashtable
        //{
            //{ "isReady", isReady },
            //{ "Water", isWater },
            //{ "Thunder", isThunder },
            //{ "Fire", isFire },
            //{ "Ground", isGround }
        //};
        
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
    }

    public void OnClickReadybutton()
    {
        isReady = !isReady;
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", isReady }
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
        int indexToRemove = playerList.FindIndex(p => p.ActorNumber == otherPlayer.ActorNumber);

        if (indexToRemove != -1)
        {
            playerList.RemoveAt(indexToRemove);

            if (ReadyObj[indexToRemove] != null)
            {
                ReadyObj[indexToRemove].SetActive(false);
            }
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