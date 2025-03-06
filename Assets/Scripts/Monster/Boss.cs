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
        // need to add -> directional light color change
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator FirstPattern()
    {
        // need to add -> PatternAniamtion
        yield return null;
    }

    protected override void MonsterDmged(float damage)
    {
        float FirPatternHealth = monsterInfo.health / 2f;
        // need to add -> hit animation
        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health < FirPatternHealth)
        {
            canMove = false;
            // need to add -> check animation, if Idle -> StartCoroutine
            StartCoroutine(FirstPattern());
        }

        if (monsterInfo.health <= 0)
        {
            // need to add -> death animation
            Destroy(gameObject);
        }
    }
}
