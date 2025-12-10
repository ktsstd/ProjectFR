using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillItem : PlayerSkill
{
    GameObject[] monsters;
    ItemUi itemUi;

    public GameObject attackSkillEffectOBJ;

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
            if (monster == null) continue;

            MonsterAI monsterAI = monster.GetComponent<MonsterAI>();

            itemUi.pv.RPC("SkillEffect",RpcTarget.All, monster.transform.position);

            yield return new WaitForSeconds(0.1f);

            if (monster == null) continue;

            if (monsterAI.monsterInfo.isBoss)
                HitOther(monster, monsterAI.MaxHp * 0.1f);
            else
                HitOther(monster, monsterAI.MaxHp * 0.5f);
        }

        PhotonNetwork.Destroy(gameObject);
    }
}