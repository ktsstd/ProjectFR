using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Sleebam : MonsterAI
{
    [SerializeField] GameObject SleebamAttackEffect;
    [SerializeField] Attackboundary atkboundary;

    public override void ShowAttackBoundary() 
    {
        atkboundary.ShowBoundary();
    }
    public override void AttackEvent()
    {
        atkboundary.EnterPlayer();
        ParticleSystem AttackE = SleebamAttackEffect.GetComponent<ParticleSystem>();
        AttackE.Play();
    }
    public override void AttackSound()
    {
        SoundManager.Instance.PlayMonsterSfx(2, transform.position);
    }
}
