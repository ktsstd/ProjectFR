using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonsterAI
{
    protected override void Attack()
    {
        Debug.Log("TestMonster Attack");
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
    }
}
