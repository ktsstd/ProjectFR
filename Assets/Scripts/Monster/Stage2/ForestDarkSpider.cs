using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestDarkSpider : MonsterAI
{
    public override void AttackEvent()
    {

    }
    public override void SkillAttack(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                Hide();
                break;
            default:
                break;
        }
        //skillTimer[skillIndex] = skillCooldown[skillIndex];
        //currentState = States.Idle;
    }
    public void Hide()
    {
        // 시작하자마자 65% 투명해지고 데미지 받으면 해제
    }
}
