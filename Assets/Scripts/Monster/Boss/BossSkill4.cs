using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill4 : MonoBehaviour
{
    private IEnumerator Start()
    {
        Boss bossScript = GameObject.FindWithTag("Enemy").GetComponent<Boss>();
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        bossScript.BossMonsterSkillTimers[3] = bossScript.BossMonsterSkillCooldowns[3];
        bossScript.monsterInfo.attackTimer = bossScript.monsterInfo.attackCooldown;
        Destroy(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            // playerS.pv.RPC("OnPlayerHit", RpcTarget.All, damage);
            // todo -> PlayerPoision
        }
    }
}
