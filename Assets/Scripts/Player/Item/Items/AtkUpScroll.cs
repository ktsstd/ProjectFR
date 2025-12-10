using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkUpScroll : PlayerItem
{
    public GameObject[] players;
    public GameObject EffectOBJ;

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
                playerinfo.StartCoroutine(playerinfo.PlayerAttackBuff(5f, 1.35f));

                GameObject effectOBJ = Instantiate(EffectOBJ);
                effectOBJ.transform.parent = playerinfo.transform;
                effectOBJ.transform.localPosition = Vector3.zero;
            }
        }
    }
}
