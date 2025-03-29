using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Solborn : MonsterAI
{
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
    }
}
