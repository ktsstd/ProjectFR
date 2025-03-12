using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonsterAI
{
    private float FirPatternbreakupHealth;
    private Transform BossMouthPos;
    private int BossPhase;
    private float[] BossMonsterSkillCooldowns = { 7f, 30f, 20f };
    private float[] BossMonsterSkillTimers = new float[3];

    protected override void Start()
    {
        base.Start();
        FirPatternbreakupHealth = monsterInfo.health / 0.2f;
        BossPhase = 1;
        // todo -> directional light color change
    }

    protected override void Update()
    {
        base.Update();
        for (int i = 0; i < BossMonsterSkillTimers.Length; i++)
        {
            if (BossMonsterSkillTimers[i] > 0f)
            {
                BossMonsterSkillTimers[i] -= Time.deltaTime;
            }
        }
    }
    protected override void Attack() // todo -> attacking animation
    {
        // string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[0].name;
        // Vector3 attackFowardPos = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1;
        // GameObject AttackObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos, Quaternion.identity);
        // AttackObj.transform.SetParent(this.transform);
        
    }

    private int GetRandomSkill()
    {
        List<int> availableSkills = new List<int>();

        for (int i = 0; i < BossMonsterSkillTimers.Length; i++)
        {
            if (BossMonsterSkillTimers[i] <= 0f) 
            {
                availableSkills.Add(i);
            }
        }

        if (availableSkills.Count > 0)
        {
            return availableSkills[Random.Range(0, availableSkills.Count)];
        }

        else
        {
            // AllSkillCooldownTimer = AllSkillCooldown;
            // isBossPatern = false;
        }
        return -1;  
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
            // StartCoroutine(FirstPattern());
        }

        if (monsterInfo.health <= 0)
        {
            // todo -> death animation
            Destroy(gameObject);
        }
    }
}
