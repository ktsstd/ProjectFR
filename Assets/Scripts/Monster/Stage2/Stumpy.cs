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
        // ���� �ִϸ��̼� 0.2�� ���ð� �ϰ� ���������� ǥ�ó��� Attackboundary �ű⿡���� �ø���ɵ�
    }
}
