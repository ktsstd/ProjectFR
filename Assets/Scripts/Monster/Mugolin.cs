using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mugolin : MonsterAI
{    
    private bool InSafeZone;
    private bool IsRolling;

    private float defaultspeed;
    private float Increasespeed;
    private float IncreasePerspeed;
    public override void Start()
    {
        base.Start();
        InSafeZone = false;
        IsRolling = false;
        //monsterInfo.attackTimer = 99999f;
        defaultspeed = agent.speed;
        Increasespeed = defaultspeed * 2;
        IncreasePerspeed = (Increasespeed - defaultspeed) / 5;
        StartCoroutine(StartMove());
    }
    public override void Update()
    {
        base.Update();
    }

    private IEnumerator StartMove() // todo -> speedup, rolling animationStart, speedup -> damageup
    {
        IsRolling = true;
        for (int i = 0; i < 5; i++)
        {
            agent.speed += IncreasePerspeed;
            yield return new WaitForSeconds(1f);
        }   
        yield break; 
    }
    
    private IEnumerator StopMove()
    {
        StopCoroutine(StartMove()); // todo -> stun Effect
        canMove = false;
        IsRolling = false;
        agent.speed = defaultspeed;
        yield return new WaitForSeconds(2f);
        canMove = true;
        StartCoroutine(StartMove());
    }

    private void OnTriggerEnter(Collision other) 
    {
        if (other.gameObject.tag == "Player" && IsRolling)
        {
            StartCoroutine(StopMove());
        }

        if (other.gameObject.tag == "SafeZone" && IsRolling) // todo -> hit with object: Stop rolling, attackTimer reset
        {
            //StartCoroutine(StopMove());
            agent.speed = defaultspeed;
            IsRolling = false;
            monsterInfo.attackTimer = monsterInfo.attackCooldown;
        }
    }

}
