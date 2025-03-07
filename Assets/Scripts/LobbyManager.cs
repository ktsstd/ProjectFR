using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomNameText;  // ���� �� �̸��� ǥ���� �ؽ�Ʈ
    public TMP_Text[] playerTexts; // �÷��̾���� �̸��� ǥ���� �ؽ�Ʈ �迭

    private List<Player> playerList = new List<Player>(); // �濡 �ִ� �÷��̾� ����Ʈ


    void Start()
    {
        // �� �̸��� ǥ��
        if (roomNameText != null)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name; // ���� �� �̸��� ǥ��
        }

        // �÷��̾���� ������ ����Ʈ�� �߰�
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.Add(player);
        }

        // UI �ʱ�ȭ
        UpdatePlayerListUI();
    }

    // �÷��̾ �������� �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);
        UpdatePlayerListUI(); // UI ����
    }

    // �÷��̾ �������� �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerList.Remove(otherPlayer);
        UpdatePlayerListUI(); // UI ����
    }

    // �÷��̾� ��� UI ����
    void UpdatePlayerListUI()
    {
        // �÷��̾� ��� UI ����
        for (int i = 0; i < playerTexts.Length; i++)
        {
            playerTexts[i].text = ""; // �ʱ�ȭ
        }

        // �濡 �ִ� �÷��̾���� ������ UI�� ǥ��
        for (int i = 0; i < playerList.Count && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerList[i].NickName;
        }
    }
}
