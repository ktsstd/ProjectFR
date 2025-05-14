using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleebam : MonsterAI
{
    [SerializeField] GameObject SleebamAttackEffect;
    public override void Attack()
    {
        base.Attack();
    }
    public override void AttackEffect()
    {
        ParticleSystem AttackE = SleebamAttackEffect.GetComponent<ParticleSystem>();
        AttackE.Play();
        SleebamAttackEffect.SetActive(true);
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
