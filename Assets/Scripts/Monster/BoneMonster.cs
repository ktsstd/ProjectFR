using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMonster : MonsterAI
{
    protected override void MonsterDmged(float damage) // need to add -> revive
    {
        if (!photonView.IsMine) return;

        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health <= 0)
        {
            //PhotonNetwork.Destroy(gameObject);
        }
    }
}
