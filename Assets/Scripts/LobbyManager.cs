using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;  // 현재 방 이름을 표시할 텍스트
    public TMP_Text[] playerTexts; // 플레이어들의 이름을 표시할 텍스트 배열

    private List<Player> playerList = new List<Player>(); // 방에 있는 플레이어 리스트


    void Start()
    {
        // 방 이름을 표시
        if (roomNameText != null)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name; // 현재 방 이름을 표시
        }

        // 플레이어들의 정보를 리스트에 추가
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
        }

        // UI 초기화
        UpdatePlayerListUI();
    }

    // 플레이어가 입장했을 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);
        UpdatePlayerListUI(); // UI 갱신
    }

    // 플레이어가 퇴장했을 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerList.Remove(otherPlayer);
        UpdatePlayerListUI(); // UI 갱신
    }

    // 플레이어 목록 UI 갱신
    void UpdatePlayerListUI()
    {
        // 플레이어 목록 UI 갱신
        for (int i = 0; i < playerTexts.Length; i++)
        {
            playerTexts[i].text = ""; // 초기화
        }

        // 방에 있는 플레이어들의 정보를 UI에 표시
        for (int i = 0; i < playerList.Count && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerList[i].NickName;
        }
    }
}
