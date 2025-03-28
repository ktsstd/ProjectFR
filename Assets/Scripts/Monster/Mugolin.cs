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
        monsterInfo.attackTimer = 99999f;
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
        animator.SetBool("isRolling", true);
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
        animator.SetTrigger("Stun");
        animator.SetBool("isRolling", false);        
        agent.velocity = Vector3.zero;
        canMove = false;
        IsRolling = false;
        if (monsterSlowCurTime > 0)
        {
            float slowSpeed = Increasespeed - agent.speed;
            agent.speed = defaultspeed - slowSpeed;
        }
        else
        {
            agent.speed = defaultspeed;
        }
        yield return new WaitForSeconds(1.333f); // todo stun duration
        canMove = true;
        StartCoroutine(StartMove());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && IsRolling)
        {
            StopCoroutine(StartMove());
            StartCoroutine(StopMove());
            agent.velocity = Vector3.zero;
        }
    }
    public void StandUp()
    {
        animator.SetBool("isRolling", false);
        animator.SetTrigger("isUp");
        agent.velocity = Vector3.zero;
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        IsRolling = false;
        if (monsterSlowCurTime > 0)
        {
            float slowSpeed = Increasespeed - agent.speed;
            agent.speed = defaultspeed - slowSpeed;
        }
        else
        {
            agent.speed = defaultspeed;
        }
    }
}
