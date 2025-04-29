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
        Debug.Log("Photon Master ������ ����Ǿ����ϴ�.");
        var ro = new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true };
        PhotonNetwork.CreateRoom("asdf", ro);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� �����߽��ϴ�.");
    }

    public void RequestJoinSelectedRoom()
    {
        Debug.Log($"Joining room: {RoomData.selectedRoomName}");
        PhotonNetwork.JoinRoom(RoomData.selectedRoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"CreateRoom Failed: {message} (�ڵ� {returnCode})");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"JoinRoom Failed: {message} (�ڵ� {returnCode})");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�뿡 �����߽��ϴ�.");
        // ���� ��� ����ȭ �� �߰� ����
    }
}
