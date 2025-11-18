using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;
    public TMP_Text[] playerTexts;
    public TMP_Text modeText;

    private List<Player> playerList = new List<Player>();
   
    int localPlayerIndex;
    //public GameObject[] CharacterImg;
    private GameObject CharacterObj;
    public GameObject CharacterImgParent;
    public GameObject[] ReadyObj;
    public GameObject StartObj;

    public Transform[] CharacterPos;
    public Transform[] CharacterUIPos;
    public TextMeshProUGUI WarningText;

    string playerPrefab = "";

    private bool isReady;

    public int CharacterIndex;
    public int selectedMode = 0; // 0 기본모드 1 무한모드

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
            SetReadyState(player, playerIsReady);
        }

        UpdatePlayerListUI();
        localPlayerIndex = GetLocalPlayerIndex();
        if (localPlayerIndex >= 0 && localPlayerIndex < CharacterUIPos.Length)
        {
            CharacterImgParent.transform.position = CharacterUIPos[localPlayerIndex].position;
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.K))
        {
            PhotonNetwork.LoadLevel("Stage2");
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
        if (CharacterIndex < 3)
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
            CharacterIndex = 3;
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
            playerPrefab = "Earth";
        }
        else if (CharacterIndex == 3)
        {
            playerPrefab = "Fire";
        }
        localPlayerIndex = GetLocalPlayerIndex();
        PhotonNetwork.Destroy(CharacterObj);
        CharacterObj = PhotonNetwork.Instantiate("Lobby/" + playerPrefab, CharacterPos[localPlayerIndex].position, Quaternion.identity, 0);

        photonView.RPC("CharacterParentSet", RpcTarget.All);
    }
    [PunRPC]
    public void CharacterParentSet()
    {
        if (localPlayerIndex  == 0)
        {
            CharacterObj.transform.rotation = Quaternion.Euler(0, 139, 0);
        }
        else if (localPlayerIndex == 1)
        {
            CharacterObj.transform.rotation = Quaternion.Euler(0, 158, 0);
        }
        else if (localPlayerIndex == 2)
        {
            CharacterObj.transform.rotation = Quaternion.Euler(0, 202, 0);
        }
        else if (localPlayerIndex == 3)
        {
            CharacterObj.transform.rotation = Quaternion.Euler(0, 221, 0);
        }
        CharacterObj.transform.localScale = new Vector3(0.4647731f, 0.4647731f, 0.4647731f);
    }

    // Ready 버튼 클릭 시 처리
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
        localPlayerIndex = GetLocalPlayerIndex();
        if (localPlayerIndex < 0 || localPlayerIndex >= CharacterUIPos.Length)
        {
            Debug.LogError("로컬 플레이어의 인덱스가 올바르지 않습니다.");
            return;
        }

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", isReady },
            { "selectedCharacter", CharacterIndex },
            { "selectedMode", selectedMode }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
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
    bool Starting = false;
    public void OnClickGameStart()
    {
        if (!Starting)
        {
            Starting = true;
            PhotonNetwork.LoadLevel("Stage1");
        }
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

    public override void OnJoinedRoom()
    {
        Player masterClient = PhotonNetwork.MasterClient;
        if (masterClient.CustomProperties.TryGetValue("selectedMode", out object modeState))
        {
            selectedMode = (int)modeState;
        }
    }
    public void OnClickLeaveRoom()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", null },
            { "selectedCharacter", -1 }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main"); // 방을 떠나면 새로운 씬으로 이동
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
            }
        }

        UpdatePlayerListUI();
        localPlayerIndex = GetLocalPlayerIndex();

        CharacterImgParent.transform.position = CharacterUIPos[localPlayerIndex].position;
        PhotonNetwork.DestroyAll(CharacterObj);
        CharacterObj = PhotonNetwork.Instantiate("Lobby/" + playerPrefab, CharacterPos[localPlayerIndex].position, Quaternion.identity, 0);
        photonView.RPC("CharacterParentSet", RpcTarget.All);
        if (localPlayerIndex >= 0 && localPlayerIndex < CharacterUIPos.Length)
        {
            
        }

    }

    private int GetLocalPlayerIndex()
    {
        return playerList.FindIndex(p => p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
    }
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
    public void OnClickModeChange(int mode) // 0 기본 1 무한
    {
        selectedMode = mode;
        photonView.RPC("ChangeSeletedMode", RpcTarget.All, selectedMode);
    }

    [PunRPC]
    public void ChangeSeletedMode(int mode)
    {
        modeText.text = selectedMode == 0 ? "기본모드: 선택됨" : "무한모드: 선택됨";
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
           { "selectedMode", selectedMode }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }
}
