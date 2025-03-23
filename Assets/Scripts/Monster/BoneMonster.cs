using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoneMonster : MonsterAI
{
    private float reviveHealth;
    public override void Start()
    {
        base.Start();
        reviveHealth = monsterInfo.health;
    }
    public override void MonsterDmged(float damage)
    {
        if (!photonView.IsMine) return;

        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health <= 0) // todo -> revive animation
        {
            if (reviveHealth > 0)
            {
                monsterInfo.health += reviveHealth;
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
