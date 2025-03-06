using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();

    private GameObject roomItemPrefab;
    public Transform scrollContent;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        roomItemPrefab = Resources.Load<GameObject>("RoomItem");

        if (roomItemPrefab == null)
        {
            Debug.LogError("RoomItem prefab이 Resources에 없습니다.");
        }
        else
        {
            Debug.Log("Photon Network에 이미 연결되었습니다.");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Master 서버에 연결되었습니다.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 접속했습니다.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"랜덤 룸 입장 실패. 코드: {returnCode}, 메시지: {message}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("룸이 성공적으로 생성되었습니다.");

        // 룸 생성 후 RoomItem을 추가
        GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContent);
        roomPrefab.GetComponent<RoomData>().RoomInfo = PhotonNetwork.CurrentRoom;
        rooms.Add(PhotonNetwork.CurrentRoom.Name, roomPrefab);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 접속했습니다.");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("현재 마스터 클라이언트입니다.");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"룸 목록 업데이트: {roomList.Count}개의 룸");

        GameObject tempRoom = null;

        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                Debug.Log($"룸 {roomInfo.Name}이(가) 삭제되었습니다.");
                if (rooms.TryGetValue(roomInfo.Name, out tempRoom))
                {
                    Destroy(tempRoom);
                    rooms.Remove(roomInfo.Name);
                    Debug.Log($"룸 {roomInfo.Name}을(를) 딕셔너리에서 제거하고 프리팹을 삭제했습니다.");
                }
            }
            else
            {
                if (!rooms.ContainsKey(roomInfo.Name))
                {
                    Debug.Log($"새로운 룸이 추가되었습니다: {roomInfo.Name}");
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContent);
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;
                    rooms.Add(roomInfo.Name, roomPrefab);
                }
                else
                {
                    Debug.Log($"룸 정보 갱신: {roomInfo.Name}");
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
                }
            }
        }
    }

    #region UI_BUTTON_EVENT

    public void OnMakeRoomClick()
    {
        Debug.Log("룸 생성 버튼 클릭됨.");

        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 4,     // 최대 접속자 수
            IsOpen = true,       // 룸 오픈 여부
            IsVisible = true     // 로비에서 룸 목록에 노출 여부
        };

        // 룸 이름을 랜덤으로 생성하여 룸 생성
        string roomName = $"ROOM_{Random.Range(1, 101):000}";
        Debug.Log($"룸 이름: {roomName}");
        PhotonNetwork.CreateRoom(roomName, ro);
    }

    #endregion
}
