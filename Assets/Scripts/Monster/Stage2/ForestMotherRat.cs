using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMotherRat : MonsterAI
{
    public override void AttackEvent()
    {

    }
    public override void SkillAttack(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                CallRat();
                break;
            default:
                break;
        }
        //skillTimer[skillIndex] = skillCooldown[skillIndex];
        //currentState = States.Idle;
    }
    public void CallRat()
    {
        // 대충 애니메이션 0.7초 포효하고 주변에 작은 쥐 페이드인으로 하라는데 미친거지 아주
    }
}
