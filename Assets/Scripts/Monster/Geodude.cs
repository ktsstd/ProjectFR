using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geodude : MonsterAI
{
    
    public override void Start()
    {
        base.Start();
        //monsterInfo.attackTimer = 99999f;
        StartCoroutine(StartMove());
    }
    public override void Update()
    {
        base.Update();
    }

    private IEnumerator StartMove() // todo -> speedup, rolling animationStart, speedup -> damageup
    {
        yield break; 
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Player") // todo -> hit with player: both stun, few sec rerolling
        {
            StopCoroutine(StartMove());
        }

        else if (other.gameObject.tag == "TestObject") // todo -> hit with object: Stop rolling, attackTimer reset
        {
            StopCoroutine(StartMove());
        }
    }
}
