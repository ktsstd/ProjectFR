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
        // �������ڸ��� 65% ���������� ������ ������ ����
    }
}
