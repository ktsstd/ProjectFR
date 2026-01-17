using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMotherRat : MonsterAI
{
    [SerializeField] Attackboundary atkboundary;
    [SerializeField] Transform mob1;
    [SerializeField] Transform mob2;
    [SerializeField] Transform mob3;
    [SerializeField] Transform mob4;
    public void Start()
    {
        currentState = States.Attack;
        animator.SetTrigger("StartSkill");
    }

    public override void SkillAttack(int skillIndex)
    {
        
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
    }
    public override void SkillEvent()
    {
        PhotonNetwork.Instantiate("Monster/Stage2/Rat_baby", mob1.position, mob1.rotation);
        PhotonNetwork.Instantiate("Monster/Stage2/Rat_baby", mob2.position, mob2.rotation);
        PhotonNetwork.Instantiate("Monster/Stage2/Rat_baby", mob3.position, mob3.rotation);
        PhotonNetwork.Instantiate("Monster/Stage2/Rat_baby", mob4.position, mob4.rotation);
        thinkTimer = thinkTime;
        currentState = States.Idle;
    }
}
