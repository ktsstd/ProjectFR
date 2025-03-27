using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIreMonster : MonsterAI
{
    public override void MonsterDmged(float damage)
    {
        //if (!photonView.IsMine) return;

        CurHp -= damage;
        Debug.Log("health: " + CurHp);
        if (CurHp <= 0)
        {
            DestroyMonster();
        }
    }
}
