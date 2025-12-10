using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopScroll : PlayerItem
{
    public GameObject[] players;
    public GameObject zonYa;

    public override void Start()
    {
        base.Start();
    }

    public override void ItemEffect(int _player, Vector3 _usePos)
    {
        SoundManager.Instance.PlayItemSfx(0, GameManager.Instance.localPlayerCharacter.transform.position);

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PlayerController playerinfo = player.GetComponent<PlayerController>();
            if (playerinfo.elementalCode == _player)
            {
                playerinfo.pv.RPC("OnPlayerSuppressed", Photon.Pun.RpcTarget.All, 2f);
                GameObject ZONYA = Instantiate(zonYa);
                ZONYA.transform.parent = playerinfo.transform;
                ZONYA.transform.localPosition = Vector3.up;
            }
        }
    }
}
