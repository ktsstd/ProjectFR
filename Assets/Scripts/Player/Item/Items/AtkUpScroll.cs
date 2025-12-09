using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkUpScroll : PlayerItem
{
    public GameObject[] players;
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
                playerinfo.StartCoroutine(playerinfo.PlayerAttackBuff(5f, 1.35f));
            }
        }
    }
}
