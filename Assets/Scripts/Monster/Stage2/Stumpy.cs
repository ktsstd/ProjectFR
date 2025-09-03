using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stumpy : MonsterAI
{
    public override void AttackEvent()
    {

    }
    public override void SkillAttack(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                StumpyRoot();
                break;
            default:
                break;
        }
        //skillTimer[skillIndex] = skillCooldown[skillIndex];
        //currentState = States.Idle;
    }
    public void StumpyRoot()
    {
        // 대충 애니메이션 0.2초 대기시간 하고 빨간색으로 표시놓고 Attackboundary 거기에놓고 올리면될듯
    }
}
