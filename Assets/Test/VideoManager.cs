using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class VideoManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Light DLight;
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        DLight.color = new Color(1f, 0.3216f, 0f, 1f);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Master 서버에 연결되었습니다.");
        var ro = new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true };
        PhotonNetwork.CreateRoom("asdf", ro);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 접속했습니다.");
    }

    public void RequestJoinSelectedRoom()
    {
        Debug.Log($"Joining room: {RoomData.selectedRoomName}");
        PhotonNetwork.JoinRoom(RoomData.selectedRoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"CreateRoom Failed: {message} (코드 {returnCode})");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"JoinRoom Failed: {message} (코드 {returnCode})");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 입장했습니다.");
        // 영상 재생 동기화 등 추가 로직
    }
}
