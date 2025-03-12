using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boss : MonsterAI
{
    private float FirPatternbreakupHealth;
    private Transform BossMouthPos;
    private int BossPhase;
    private float[] BossMonsterSkillCooldowns = { 1f, 7f, 30f, 20f };
    private float[] BossMonsterSkillTimers = new float[4];

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
    protected override void Attack() 
    {
        int randomskill = GetRandomSkill();
        string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[randomskill].name; 
        switch(randomskill)
        {
            case 0:
                // todo -> attacking animation
                Vector3 attackFowardPos1 = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1;
                GameObject AttackObj1 = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos1, Quaternion.identity);
                AttackObj1.transform.SetParent(this.transform);
                break;
            case 1:
                // todo -> jumping animation, jumppos tp (thinkabout)
                Vector3 attackFowardPos2 = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1;
                GameObject AttackObj2 = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos2, Quaternion.identity);
                AttackObj2.transform.SetParent(this.transform);
                break;
            case 2:
                // todo -> Eat (thinkabout)
                break;
            case 3:
                // todo -> poision (thinkabout)
                break;
            default:
                canMove = true;
                break;    
            
        }
    }

    private int GetRandomSkill()
    {
        List<int> availableSkills = new List<int>();

        for (int i = 0; i < BossMonsterSkillTimers.Length; i++)
        {
            if (BossMonsterSkillTimers[i] <= 0f)
            {
                if (i == 2 || i == 3)
                {
                    if (BossPhase >= 2)
                    {
                        availableSkills.Add(i);
                    }
                }
                else
                {
                    availableSkills.Add(i);
                }
                
            }
        }

        if (availableSkills.Count > 0)
        {
            return availableSkills[Random.Range(0, availableSkills.Count)];
        }

        else
        {
            monsterInfo.attackTimer = monsterInfo.attackCooldown;
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
