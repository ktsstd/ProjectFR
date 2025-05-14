using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using JetBrains.Annotations;

public class Solborn : MonsterAI
{
    [SerializeField] ParticleSystem SolbornEffect;
    public override void Start()
    {
        base.Start();
        animator.SetTrigger("Spawn");
        canMove = false;
        StartCoroutine(StartMove());
    }
    private IEnumerator StartMove()
    {
        yield return new WaitForSeconds(1.1f);
        canMove = true;
    }
    public override void Attack() // todo -> attacking animation
    {
        base.Attack();
    }
    public override void AttackEffect()
    {
        SolbornEffect.Play();
    }
    public override void AttackAnimation()
    {
        animator.SetTrigger("StartAttack");
        Debug.Log("AttackAnimation Called");
    }
    public override void AttackSound() 
    {        
        SoundManager.Instance.PlayMonsterSfx(1, transform.position);
    }

    private IEnumerator SolbornAttack()
    {
        attackboundary.SetActive(true);
        Attackboundary attackboundaryScript = attackboundary.GetComponent<Attackboundary>();
        //attackboundaryScript.Starting(attackSpeed);
        yield return new WaitForSeconds(1.17f);
        if (animator != null)
            animator.SetTrigger("StartAttack");
        yield return new WaitForSeconds(0.2f);
    }
}
