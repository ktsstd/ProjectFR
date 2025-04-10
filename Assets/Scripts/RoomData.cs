using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
    [SerializeField]
    private TMP_Text roomInfoText;
    private PhotonManager photonManager;
    public static string selectedRoomName;
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // 룸 정보 표시
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
        }
    }

    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        
    }

    public void OnClickRoomSelected()
    {
        selectedRoomName = _roomInfo.Name;
    }
}