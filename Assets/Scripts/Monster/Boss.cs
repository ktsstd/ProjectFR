using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonsterAI
{
    private float FirPatternbreakupHealth;
    private Transform BossMouthPos;

    protected override void Start()
    {
        base.Start();
        FirPatternbreakupHealth = monsterInfo.health / 0.2f;
        // todo -> directional light color change
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator FirstPattern()
    {
        // todo -> PatternAniamtion
        yield return null;
    }

    protected override void MonsterDmged(float damage)
    {
        float FirPatternHealth = monsterInfo.health / 2f;
        // todo -> hit animation
        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health < FirPatternHealth)
        {
            canMove = false;
            // todo -> check animation, if Idle -> StartCoroutine
            StartCoroutine(FirstPattern());
        }

        if (monsterInfo.health <= 0)
        {
            // todo -> death animation
            Destroy(gameObject);
        }
    }
}
