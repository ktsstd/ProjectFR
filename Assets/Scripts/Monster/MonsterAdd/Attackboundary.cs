using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attackboundary : MonoBehaviour
{
    [SerializeField] MonsterAI monsterAIScript;
    [SerializeField] Collider thiscollider;
    HashSet<GameObject> damagedTargets = new HashSet<GameObject>();

    public void Starting(float onTime, float delayTime)
    {
        damagedTargets.Clear();
        thiscollider.enabled = false;
        StartCoroutine(ShowAttackBoundary(onTime, delayTime));
    }

    IEnumerator ShowAttackBoundary(float onTime, float delayTime)
    {
        yield return new WaitForSeconds(onTime);
        monsterAIScript.AttackAnimation();
        
        yield return new WaitForSeconds(delayTime);
        monsterAIScript.AttackEffect();

        thiscollider.enabled = true;
        yield return new WaitForSeconds(0.1f);

        thiscollider.enabled = false;
        monsterAIScript.photonView.RPC("AfterAttack", RpcTarget.All);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (damagedTargets.Contains(other.gameObject)) return;

        if (other.CompareTag("Player"))
        {
            damagedTargets.Add(other.gameObject);
            PlayerController playerS = other.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerHit", RpcTarget.All, monsterAIScript.damage);
            monsterAIScript.AttackSound();
        }
        else if (other.CompareTag("Object"))
        {
            damagedTargets.Add(other.gameObject);
            Object objectS = other.GetComponent<Object>();
            objectS.Damaged(monsterAIScript.damage);
            monsterAIScript.AttackSound();
        }
    }

}
