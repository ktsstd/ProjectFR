using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScroll : PlayerItem
{
    public GameObject[] monsters;
    public override void Start()
    {
        base.Start();
    }
    public override void ItemEffect(int _player, Vector3 _usePos)
    {
        monsters = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject monster in monsters)
        {
            monster.GetComponent<MonsterAI>().OnMonsterSpeedDown(5f, 3f);
        }
    }
}