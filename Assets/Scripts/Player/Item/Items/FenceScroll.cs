using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceScroll : PlayerItem
{
    GameObject[] players;
    public override void Start()
    {
        base.Start();
    }

    public override void ItemEffect(int _player, Vector3 _usePos)
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PlayerController playerinfo = player.GetComponent<PlayerController>();
            if (playerinfo.elementalCode == _player)
            {
                Quaternion newRot = player.transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
                PhotonNetwork.Instantiate("Obstacles/Fence", _usePos, newRot);
            }
        }
    }
}
