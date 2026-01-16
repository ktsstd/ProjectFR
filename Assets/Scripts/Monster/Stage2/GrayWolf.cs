using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayWolf : MonsterAI
{
    //[SerializeField] GameObject AttackEffect;
    [SerializeField] Attackboundary atkboundary;
    [SerializeField] Attackboundary skillboundary;
    //if (currentState == States.Die) return;
    //    skillTimer[skillIndex] = skillCooldown[skillIndex];
    //    thinkTimer = thinkTime;
    //    currentState = States.Idle;
    public override void SkillAttack(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                GrayWolfDash();
                break;
            default:
                break;
        }
        //skillTimer[skillIndex] = skillCooldown[skillIndex];
        //currentState = States.Idle;
    }
    public void GrayWolfDash()
    {
        if (currentState != States.Attack) return;
        animator.SetTrigger("StartSkill");
        ShowSkillBoundary();
    }
    public override void ShowAttackBoundary()
    {
        if (currentState != States.Attack) return;
        atkboundary.ShowBoundary();
    }
    public override void AttackEvent()
    {
        if (currentState != States.Attack) return;
        atkboundary.EnterPlayer();
        //ParticleSystem AttackE = SleebamAttackEffect.GetComponent<ParticleSystem>();
        //AttackE.Play();
    }
    public override void ShowSkillBoundary()
    {
        if (currentState != States.Attack) return;
        skillboundary.EnterPlayer();
    }
    public override void SkillEvent()
    {
        if (currentState != States.Attack) return;
        atkboundary.EnterPlayer();
    }
}
