using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AttackSkillItem : PlayerSkill
{
    GameObject[] monsters;
    ItemUi itemUi;
    public override void Start()
    {
        base.Start();

        playerCode = 4;
        monsters = GameObject.FindGameObjectsWithTag("Enemy");
        itemUi = FindObjectOfType<ItemUi>();
        StartCoroutine("Skill");
    }

    IEnumerator Skill()
    {
        foreach (GameObject monster in monsters)
        {
            MonsterAI monsterAI = monster.GetComponent<MonsterAI>();

            itemUi.pv.RPC("AttackSkillEffect", Photon.Pun.RpcTarget.All, monster.transform.position);
            yield return new WaitForSeconds(0.1f);

            if (monsterAI.monsterInfo.isBoss)
                HitOther(monster, monsterAI.MaxHp * 0.1f);
            else
                HitOther(monster, monsterAI.MaxHp * 0.5f);
        }

        Destroy(gameObject);
    }
}