using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Sleebam : MonsterAI
{
    [SerializeField] GameObject SleebamAttackEffect;
    [SerializeField] Attackboundary atkboundary;
    public override void Attack()
    {
        base.Attack();
        
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

    //private IEnumerator SleebamAttack()
    //{
    //    attackboundary.SetActive(true);
    //    Attackboundary attackboundaryScript = attackboundary.GetComponent<Attackboundary>();
    //    attackboundaryScript.Starting(attackSpeed);
    //    yield return new WaitForSeconds(1f);
    //    if (animator != null)
    //        animator.SetTrigger("StartAttack");
    //    yield return new WaitForSeconds(0.15f);
    //    SoundManager.Instance.PlayMonsterSfx(2, transform.position); // Edit
    //}
}
