using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMonster : MonsterAI
{
    protected override void Attack() // todo -> attacking animation
    {
        string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[0].name;
        Vector3 attackFowardPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        animator.SetTrigger("StartAttack");
        GameObject AttackObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos, Quaternion.identity);
        AttackObj.transform.SetParent(this.transform);
    }
}
