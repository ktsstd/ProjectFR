using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geodude : MonsterAI
{
    protected override void Start()
    {
        base.Start();
        monsterInfo.attackTimer = 99999f;
        StartCoroutine(StartMove());
    }
    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator StartMove() // need to add -> speedup, rolling animationStart, speedup -> damageup
    {
        yield break; 
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Player") // need to add -> hit with player: both stun, few sec rerolling
        {
            StopCoroutine(StartMove());
        }

        else if (other.gameObject.tag == "TestObject") // need to add -> hit with object: Stop rolling, attackTimer reset
        {
            StopCoroutine(StartMove());
        }
    }
}
