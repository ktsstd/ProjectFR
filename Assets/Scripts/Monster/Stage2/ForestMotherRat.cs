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
        // ���� �ִϸ��̼� 0.7�� ��ȿ�ϰ� �ֺ��� ���� �� ���̵������� �϶�µ� ��ģ���� ����
    }
}
