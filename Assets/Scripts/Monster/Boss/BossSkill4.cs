using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill4 : MonoBehaviour
{
    Drog bossScript;
    private IEnumerator Start()
    {
        Drog bossScript = GameObject.FindWithTag("Enemy").GetComponent<Drog>();
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        bossScript.BossMonsterSkillTimers[3] = bossScript.BossMonsterSkillCooldowns[3];
        bossScript.attackTimer = bossScript.attackCooldown;
        bossScript.canMove = true;
        DestroyImmediate(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            playerS.pv.RPC("OnPlayerPoison", RpcTarget.All, 10);
            // playerS.pv.RPC("OnPlayerHit", RpcTarget.All, damage);
            // todo -> PlayerPoision
        }
    }
}
