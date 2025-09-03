using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwampSlime : MonsterAI
{
    [SerializeField] Transform ProjPos;
    [SerializeField] GameObject ProjObj;
    public override void SkillAttack(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                SlimeProjectile();
                break;
            default:
                break;
        }
        //skillTimer[skillIndex] = skillCooldown[skillIndex];
        //currentState = States.Idle;
    }

    private void SlimeProjectile()
    {
        //PhotonNetwork.Instantiate("MonsterAdd/")
        Instantiate(ProjObj, ProjPos);
    }
}
