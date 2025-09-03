using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwampSlime : MonsterAI
{
    [SerializeField] Transform ProjPos;
    [SerializeField] GameObject ProjObj;
    [SerializeField] Attackboundary atkboundary;
    public override void Attack()
    {
        base.Attack();
        currentState = States.Idle;
        attackTimer = attackCooldown;
    }

    public override void AttackEvent()
    {
        Instantiate(ProjObj, ProjPos.position, ProjPos.rotation);
    }
}
