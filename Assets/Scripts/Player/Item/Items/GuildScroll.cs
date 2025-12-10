using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildScroll : PlayerItem
{
    public GameObject[] players;
    public GameObject guildIcon;

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
                GameObject objects = GameObject.FindWithTag("Object");
                objects.GetComponent<Object>().photonView.RPC("HealthRecovery", Photon.Pun.RpcTarget.All, 0.1f);

                GameObject guild = Instantiate(guildIcon);
                guild.transform.parent = playerinfo.transform;
                guild.transform.localPosition = Vector3.up * 2;
            }
        }
    }
}
