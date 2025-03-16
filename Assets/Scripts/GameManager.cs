using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnPos;
    [SerializeField]
    private Transform SpawnPos2;
    // Start is called before the first frame update
    void Start()
    {
        string prefabName = "Water";
        PhotonNetwork.Instantiate(prefabName, SpawnPos.position, Quaternion.identity);
        string prefabName2 = "TestBoss";
        PhotonNetwork.Instantiate(prefabName2, SpawnPos2.position, Quaternion.identity);
    }
}
