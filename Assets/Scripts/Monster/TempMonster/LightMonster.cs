using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMonster : MonsterAI
{
    [SerializeField] GameObject LightMonsterBomb;
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void Attack() // todo -> attacking animation
    {
        string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[0].name;
        Vector3 attackFowardPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        animator.SetTrigger("StartAttack");
        GameObject AttackObj = Instantiate(LightMonsterBomb, attackFowardPos, Quaternion.identity);
        AttackObj.transform.SetParent(this.transform);
    }
}
