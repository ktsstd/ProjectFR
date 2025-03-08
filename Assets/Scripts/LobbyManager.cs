using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;  // ���� �� �̸��� ǥ���� �ؽ�Ʈ
    public TMP_Text[] playerTexts; // �÷��̾���� �̸��� ǥ���� �ؽ�Ʈ �迭

    private List<Player> playerList = new List<Player>(); // �濡 �ִ� �÷��̾� ����Ʈ
    public GameObject[] ReadyObj; // �÷��̾��� �غ� ���¸� ǥ���� ���� ������Ʈ �迭
    private bool isReady;

    // ������ �� ����Ǵ� �޼���
    private void Start()
    {
        isReady = false;

        // �� �̸��� ǥ��
        if (roomNameText != null)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name; // ���� �� �̸��� ǥ��
        }

        // �濡 �ִ� ��� �÷��̾���� ������ playerList�� �߰�
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);

            // �� �÷��̾��� �غ� ���¸� ������
            bool playerIsReady = player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"];
            SetReadyState(player.ActorNumber, playerIsReady); // �غ� ���¸� �ݿ�
        }

        // UI �ʱ�ȭ (�÷��̾� �̸� ������Ʈ)
        UpdatePlayerListUI();
    }

    // �غ� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnClickReadybutton()
    {
        isReady = !isReady;

        // RPC�� ���� ��� Ŭ���̾�Ʈ���� �غ� ���� ������ ����
        photonView.RPC("ReadyRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, isReady);

        // ���� �÷��̾��� �غ� ���¸� CustomProperties�� ����
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["isReady"] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // RPC�� �غ� ���¸� �����ϴ� �޼���
    [PunRPC]
    public void ReadyRPC(int playerActorNumber, bool isReady)
    {
        // ��� �÷��̾ ��ȸ�ϸ鼭 �ش� �÷��̾��� �غ� ���¸� ������Ʈ
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // �ش� �÷��̾��� ActorNumber�� ��ġ�ϸ�, �ش� �÷��̾��� �غ� ���¿� �°� ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
            if (PhotonNetwork.PlayerList[i].ActorNumber == playerActorNumber)
            {
                // ReadyObj[i]�� �÷��̾��� �غ� ���¸� ǥ���ϴ� ���� ������Ʈ
                if (ReadyObj[i] != null)
                {
                    ReadyObj[i].SetActive(isReady); // isReady�� ���� ������Ʈ Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
                }

                // �÷��̾��� �غ� ���¸� CustomProperties�� ����
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties["isReady"] = isReady;
                PhotonNetwork.PlayerList[i].SetCustomProperties(properties);

                break; // �ش� �÷��̾ ã������ �� �̻� ��ȸ�� �ʿ� ����
            }
        }
    }

    // ���ο� �÷��̾ �濡 �������� �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);  // �÷��̾� ����Ʈ�� ���� ������ �÷��̾� �߰�

        // ���� ������ �÷��̾��� �غ� ���¸� CustomProperties���� ��������
        bool playerIsReady = newPlayer.CustomProperties.ContainsKey("isReady") && (bool)newPlayer.CustomProperties["isReady"];
        SetReadyState(newPlayer.ActorNumber, playerIsReady);  // �غ� ���¸� ����

        UpdatePlayerListUI();       // UI ����
    }

    // �÷��̾ ���� ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerList.Remove(otherPlayer);  // �÷��̾� ����Ʈ���� ������ �÷��̾� ����

        // ������ �÷��̾��� �غ� ���� UI �ʱ�ȭ
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // ������ �÷��̾ ã�� �ش� �÷��̾��� ReadyObj�� ��Ȱ��ȭ
            if (PhotonNetwork.PlayerList[i].ActorNumber == otherPlayer.ActorNumber)
            {
                // ReadyObj[i]�� null�� �ƴ� ��쿡�� ��Ȱ��ȭ
                if (ReadyObj[i] != null)
                {
                    SetReadyState(i, false);
                    ReadyObj[i].SetActive(false); // ������ �÷��̾��� �غ� ������Ʈ ��Ȱ��ȭ
                }

                // ������ �÷��̾��� �غ� ���¸� false�� ����
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                properties["isReady"] = false;  // �غ� ���� false�� ����
                PhotonNetwork.PlayerList[i].SetCustomProperties(properties);

                break;
            }
        }

        // UI ����
        UpdatePlayerListUI();  // �÷��̾� ��� UI ����
    }

    // �ش� �÷��̾��� �غ� ���¸� ������Ʈ
    private void SetReadyState(int playerActorNumber, bool isReady)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == playerActorNumber)
            {
                if (ReadyObj[i] != null)
                {
                    ReadyObj[i].SetActive(isReady); // isReady�� ���� ������Ʈ Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
                }
                break;
            }
        }
    }


    // �÷��̾� ��� UI�� �����ϴ� �޼���
    void UpdatePlayerListUI()
    {
        // �÷��̾� ��� UI �ʱ�ȭ
        for (int i = 0; i < playerTexts.Length; i++)
        {
            playerTexts[i].text = ""; // �ؽ�Ʈ �ʱ�ȭ
        }

        // �濡 �ִ� �÷��̾���� ������ UI�� ǥ��
        for (int i = 0; i < playerList.Count && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerList[i].NickName; // �÷��̾��� �̸��� ǥ��
        }
    }
}
