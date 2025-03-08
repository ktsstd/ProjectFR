using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;  // 현재 방 이름을 표시할 텍스트
    public TMP_Text[] playerTexts; // 플레이어들의 이름을 표시할 텍스트 배열

    private List<Player> playerList = new List<Player>(); // 방에 있는 플레이어 리스트
    public GameObject[] ReadyObj; // 플레이어의 준비 상태를 표시할 게임 오브젝트 배열
    private bool isReady;

    // 시작할 때 실행되는 메서드
    private void Start()
    {
        isReady = false;

        // 방 이름을 표시
        if (roomNameText != null)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name; // 현재 방 이름을 표시
        }

        // 방에 있는 모든 플레이어들의 정보를 playerList에 추가
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);

            // 각 플레이어의 준비 상태를 가져옴
            bool playerIsReady = player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"];
            SetReadyState(player.ActorNumber, playerIsReady); // 준비 상태를 반영
        }

        // UI 초기화 (플레이어 이름 업데이트)
        UpdatePlayerListUI();
    }

    // 준비 버튼 클릭 시 호출되는 메서드
    public void OnClickReadybutton()
    {
        isReady = !isReady;

        // RPC를 통해 모든 클라이언트에게 준비 상태 변경을 전달
        photonView.RPC("ReadyRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, isReady);

        // 로컬 플레이어의 준비 상태를 CustomProperties로 저장
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["isReady"] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // RPC로 준비 상태를 변경하는 메서드
    [PunRPC]
    public void ReadyRPC(int playerActorNumber, bool isReady)
    {
        // 모든 플레이어를 순회하면서 해당 플레이어의 준비 상태를 업데이트
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // 해당 플레이어의 ActorNumber가 일치하면, 해당 플레이어의 준비 상태에 맞게 오브젝트 활성화/비활성화
            if (PhotonNetwork.PlayerList[i].ActorNumber == playerActorNumber)
            {
                // ReadyObj[i]는 플레이어의 준비 상태를 표시하는 게임 오브젝트
                if (ReadyObj[i] != null)
                {
                    ReadyObj[i].SetActive(isReady); // isReady에 따라 오브젝트 활성화 또는 비활성화
                }

                // 플레이어의 준비 상태를 CustomProperties로 저장
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties["isReady"] = isReady;
                PhotonNetwork.PlayerList[i].SetCustomProperties(properties);

                break; // 해당 플레이어를 찾았으면 더 이상 순회할 필요 없음
            }
        }
    }

    // 새로운 플레이어가 방에 입장했을 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);  // 플레이어 리스트에 새로 입장한 플레이어 추가

        // 새로 입장한 플레이어의 준비 상태를 CustomProperties에서 가져오기
        bool playerIsReady = newPlayer.CustomProperties.ContainsKey("isReady") && (bool)newPlayer.CustomProperties["isReady"];
        SetReadyState(newPlayer.ActorNumber, playerIsReady);  // 준비 상태를 갱신

        UpdatePlayerListUI();       // UI 갱신
    }

    // 플레이어가 방을 떠났을 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerList.Remove(otherPlayer);  // 플레이어 리스트에서 퇴장한 플레이어 제거

        // 퇴장한 플레이어의 준비 상태 UI 초기화
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // 퇴장한 플레이어를 찾고 해당 플레이어의 ReadyObj를 비활성화
            if (PhotonNetwork.PlayerList[i].ActorNumber == otherPlayer.ActorNumber)
            {
                // ReadyObj[i]가 null이 아닌 경우에만 비활성화
                if (ReadyObj[i] != null)
                {
                    SetReadyState(i, false);
                    ReadyObj[i].SetActive(false); // 퇴장한 플레이어의 준비 오브젝트 비활성화
                }

                // 퇴장한 플레이어의 준비 상태를 false로 설정
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties["isReady"] = false;  // 준비 상태 false로 설정
                PhotonNetwork.PlayerList[i].SetCustomProperties(properties);

                break;
            }
        }

        // UI 갱신
        UpdatePlayerListUI();  // 플레이어 목록 UI 갱신
    }

    // 해당 플레이어의 준비 상태를 업데이트
    private void SetReadyState(int playerActorNumber, bool isReady)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == playerActorNumber)
            {
                if (ReadyObj[i] != null)
                {
                    ReadyObj[i].SetActive(isReady); // isReady에 따라 오브젝트 활성화 또는 비활성화
                }
                break;
            }
        }
    }


    // 플레이어 목록 UI를 갱신하는 메서드
    void UpdatePlayerListUI()
    {
        // 플레이어 목록 UI 초기화
        for (int i = 0; i < playerTexts.Length; i++)
        {
            playerTexts[i].text = ""; // 텍스트 초기화
        }

        // 방에 있는 플레이어들의 정보를 UI에 표시
        for (int i = 0; i < playerList.Count && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerList[i].NickName; // 플레이어의 이름을 표시
        }
    }
}
