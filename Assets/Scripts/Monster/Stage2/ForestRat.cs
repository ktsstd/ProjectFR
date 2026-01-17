using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestRat : MonsterAI
{
    [SerializeField] Attackboundary atkboundary;
    public override void ShowAttackBoundary()
    {
        if (currentState != States.Attack) return;
        atkboundary.ShowBoundary();
    }
    public override void AttackEvent()
    {
        if (currentState != States.Attack) return;
        atkboundary.EnterPlayer();
    }
}
