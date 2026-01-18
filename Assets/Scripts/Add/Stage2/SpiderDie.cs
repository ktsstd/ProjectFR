using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            playerS.pv.RPC("OnPlayerPoison", RpcTarget.All, 10);
        }
    }
}
