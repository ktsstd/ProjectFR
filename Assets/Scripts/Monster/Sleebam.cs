using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleebam : MonsterAI
{
    public override void Attack() // todo -> attacking animation
    {
        StartCoroutine(SleebamAttack());
    }

    private IEnumerator SleebamAttack()
    {
        AttackBoundary.SetActive(true);
        Attackboundary attackboundaryScript = AttackBoundary.GetComponent<Attackboundary>();
        attackboundaryScript.Starting();
        yield return new WaitForSeconds(1f);
        if (animator != null)
            animator.SetTrigger("StartAttack");
        yield return new WaitForSeconds(0.15f);
        SoundManager.Instance.PlayMonsterSfx(2, transform.position); // Edit
    }
}
