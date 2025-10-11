using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using System.Collections;

public class VideoManager : MonoBehaviourPunCallbacks
{
    [Header("Cinemachine")]
    public CinemachineDollyCart dollyCart;    
    float triggerPosition = 4.0f;       
    float triggerTolerance = 0.1f;      

    [Header("Post Processing")]
    public PostProcessVolume postProcessVolume;

    [Header("Vignette Settings")]
    float maxIntensity = 0.4f;
    float pulseDuration = 0.3f;
    int repeatCount = 2;

    private Vignette vignette;
    private bool hasTriggered = false;

    [Header("Light Settings")]
    [SerializeField] private Light DLight;

    [Header("First SceneroTl")]
    [SerializeField] GameObject FisrtSceneMob;
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        FisrtSceneMob.SetActive(false);
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out vignette))
        {
            vignette.intensity.value = 0f;
            vignette.enabled.value = true;
        }
        else
        {
            Debug.LogError("..");
        }
    }

    private void Update()
    {
        if (hasTriggered || vignette == null) return;

        float currentPos = dollyCart.m_Position;

        if (Mathf.Abs(currentPos - triggerPosition) <= triggerTolerance)
        {
            hasTriggered = true;
            FisrtSceneMob.SetActive(true);
            StartCoroutine(PulseVignette());

        }
    }
    private IEnumerator PulseVignette()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            // 점점 어두워짐
            yield return StartCoroutine(AnimateVignette(0f, maxIntensity, pulseDuration));
            // 점점 밝아짐
            yield return StartCoroutine(AnimateVignette(maxIntensity, 0f, pulseDuration));
        }
    }
    private IEnumerator AnimateVignette(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            vignette.intensity.value = Mathf.Lerp(from, to, t);
            yield return null;
        }

        vignette.intensity.value = to; // 마지막 값 보정
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
        VideoStart();
    }

    public void VideoStart()
    {

    }
}
