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
        bossScript.skillTimer[3] = bossScript.skillCooldown[3];
        bossScript.attackTimer = bossScript.attackCooldown;
        bossScript.currentState = Drog.States.Idle;
        DestroyImmediate(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            playerS.pv.RPC("OnPlayerPoison", RpcTarget.All, 10);
        }
    }
}
