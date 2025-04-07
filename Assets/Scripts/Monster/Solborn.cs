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
        StartCoroutine(SolbornAttack());
    }

    private IEnumerator SolbornAttack()
    {
        AttackBoundary.SetActive(true);
        Attackboundary attackboundaryScript = AttackBoundary.GetComponent<Attackboundary>();
        attackboundaryScript.Starting();
        yield return new WaitForSeconds(1.17f);
        if (animator != null)
            animator.SetTrigger("StartAttack");
        yield return new WaitForSeconds(0.2f);
        SolbornEffect.Play();
        SoundManager.Instance.PlayMonsterSfx(1, transform.position); // Edit
    }
}
