using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public Transform SpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        string prefabName = "TestPlayer";
        PhotonNetwork.Instantiate(prefabName, SpawnPos.position, Quaternion.identity);
        string prefabName2 = "TestMonsterG";
        PhotonNetwork.Instantiate(prefabName2, SpawnPos.position, Quaternion.identity);
    }
}
