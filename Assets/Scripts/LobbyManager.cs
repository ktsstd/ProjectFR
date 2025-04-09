using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
    public Transform[] CharacterUIPos;
    public TextMeshProUGUI WarningText;
    string playerPrefab = "";

    private GameObject CharacterObj;

    private void Start()
    {
        CharacterIndex = -1;
        isReady = false;

        if (roomNameText != null)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        }

        // 기존 플레이어 리스트 초기화
        playerList.Clear();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
            bool playerIsReady = player.CustomProperties.TryGetValue("isReady", out object readyState) && (bool)readyState;
            // UI 슬롯은 플레이어 리스트의 순서를 기준으로 함.
            SetReadyState(player, playerIsReady);
        }

        UpdatePlayerListUI();
        // 로컬 플레이어의 인덱스를 playerList에서 찾은 후 해당 슬롯의 위치로 이동
        int localPlayerIndex = GetLocalPlayerIndex();
        if (localPlayerIndex >= 0 && localPlayerIndex < CharacterUIPos.Length)
        {
            CharacterImgParent.transform.position = CharacterUIPos[localPlayerIndex].position;
            CharacterObj.transform.position = CharacterPos[localPlayerIndex].position;
        }
    }

    // 오른쪽 캐릭터 선택
    public void OnClickCharacterSelectRightButton()
    {
        if (isReady)
        {
            WarningTexts(3);
            return;
        }
        if (CharacterIndex < CharacterImg.Length - 1)
        {
            CharacterIndex += 1;
        }
        else
        {
            CharacterIndex = 0;
        }
        UpdateCharacterImg();
    }

    // 왼쪽 캐릭터 선택
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
            CharacterIndex = CharacterImg.Length - 1;
        }
        UpdateCharacterImg();
    }

    // 캐릭터 이미지 활성화/비활성화
    private void UpdateCharacterImg()
    {
        if (CharacterIndex == 0)
        {
            playerPrefab = "Water";
        }
        else if (CharacterIndex == 1)
        {
            playerPrefab = "Lightning";
        }
        else if (CharacterIndex == 2)
        {
            playerPrefab = "Fire";
        }
        else if (CharacterIndex == 3)
        {
            playerPrefab = "Fire";
        }
        int localPlayerIndex = GetLocalPlayerIndex();
        for (int i = 0; i < CharacterImg.Length; i++)
        {
            if (CharacterImg[i] != null)
            {
                PhotonNetwork.Destroy(CharacterObj);
            }
        }
        //if (CharacterIndex >= 0 && CharacterIndex < CharacterImg.Length)
        CharacterObj = PhotonNetwork.Instantiate("Lobby/" + playerPrefab, CharacterPos[localPlayerIndex].position, Quaternion.identity, 0);
        photonView.RPC("CharacterParentSet", RpcTarget.AllBuffered);

    }
    [PunRPC]
    public void CharacterParentSet()
    {
        int localPlayerIndex = GetLocalPlayerIndex();
        CharacterObj.transform.SetParent(CharacterPos[localPlayerIndex].transform, false);
        CharacterObj.transform.localPosition = Vector3.zero;
    }

    // Ready 버튼 클릭 시 처리
    public void OnClickReadybutton()
    {
        if (CharacterIndex == -1)
        {
            WarningTexts(2);
            return;
        }

        // 다른 플레이어와 캐릭터 중복 선택 체크 (playerList를 순회)
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
        int localPlayerIndex = GetLocalPlayerIndex();
        if (localPlayerIndex < 0 || localPlayerIndex >= CharacterUIPos.Length)
        {
            Debug.LogError("로컬 플레이어의 인덱스가 올바르지 않습니다.");
            return;
        }

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", isReady },
            { "selectedCharacter", CharacterIndex }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }
    public void OnClickUISfx()
    {
        SoundManager.Instance.PlayUISfxShot(0);
    }

    // 경고 메시지 처리
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

    // 플레이어 프로퍼티가 업데이트 될 때 UI 갱신 (매핑 시 playerList 기준 사용)
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("isReady"))
        {
            bool playerIsReady = (bool)changedProps["isReady"];
            SetReadyState(targetPlayer, playerIsReady);
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
        SetReadyState(newPlayer, playerIsReady);

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

            if (indexToRemove < ReadyObj.Length && ReadyObj[indexToRemove] != null)
            {
                ReadyObj[indexToRemove].SetActive(false);
                PhotonNetwork.Destroy(CharacterObj);
            }
        }

        UpdatePlayerListUI();

        // 로컬 플레이어의 UI 위치 갱신 (playerList 순서를 기준)
        int localPlayerIndex = GetLocalPlayerIndex();
        if (localPlayerIndex >= 0 && localPlayerIndex < CharacterUIPos.Length)
        {
            CharacterObj = PhotonNetwork.Instantiate("Lobby/" + playerPrefab, CharacterPos[localPlayerIndex].position, Quaternion.identity, 0);
            CharacterImgParent.transform.position = CharacterUIPos[localPlayerIndex].position;
            CharacterObj.transform.position = CharacterPos[localPlayerIndex].position;            
        }
    }

    // playerList에서 해당 플레이어의 인덱스 반환
    private int GetLocalPlayerIndex()
    {
        return playerList.FindIndex(p => p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
    }

    // Ready 상태를 설정할 때 playerList 기준 인덱스 사용
    private void SetReadyState(Player player, bool isReady)
    {
        int index = playerList.FindIndex(p => p.ActorNumber == player.ActorNumber);
        if (index != -1 && index < ReadyObj.Length && ReadyObj[index] != null)
        {
            ReadyObj[index].SetActive(isReady);
        }
    }

    // 모든 플레이어가 준비됐는지 여부
    private bool isAllPlayerReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.TryGetValue("isReady", out object ready) || !(bool)ready)
            {
                return false;
            }
        }
        return true;
    }

    // 플레이어 리스트 UI 업데이트 : 플레이어 리스트 순서대로 텍스트 할당
    void UpdatePlayerListUI()
    {
        // playerList를 정렬하거나 순서를 새로 할당할 수 있습니다.
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
