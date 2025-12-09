using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attackboundary : MonoBehaviour
{
    [SerializeField] MonsterAI monsterAIScript;
    [SerializeField] Collider thiscollider;
    HashSet<GameObject> damagedTargets = new HashSet<GameObject>();

    private void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x, monsterAIScript.attackRange, transform.localScale.z);
    }
    public void ShowBoundary()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.8f);
    }
    public void HideBoundary()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
    }
    public void EnterPlayer()
    {
        HideBoundary();
        thiscollider.enabled = true;
        //monsterAIScript.photonView.RPC("AfterAttack", RpcTarget.All);
        //Effect.SetActive(true);
        Invoke("OffCollider", 0.1f);    
    }
    private void OffCollider()
    {
        thiscollider.enabled = false;
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        damagedTargets.Clear();
        if (monsterAIScript.currentState != MonsterAI.States.Die)
        {
            monsterAIScript.currentState = MonsterAI.States.Idle;
        } 
        monsterAIScript.attackTimer = monsterAIScript.attackCooldown;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (damagedTargets.Contains(other.gameObject)) return;

        if (other.CompareTag("Player"))
        {
            damagedTargets.Add(other.gameObject);
            PlayerController playerS = other.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerHit", RpcTarget.All, monsterAIScript.damage);
        }
        else if (other.CompareTag("Object"))
        {
            damagedTargets.Add(other.gameObject);
            Object objectS = other.GetComponent<Object>();
            objectS.Damaged(monsterAIScript.damage);
        }

        else if (other.CompareTag("Summon"))
        {
            damagedTargets.Add(other.gameObject);
            SummonAI summonS = other.GetComponent<SummonAI>();
            summonS.photonView.RPC("OnSummonHit", RpcTarget.All, monsterAIScript.damage);
        }

        else if (other.CompareTag("Obstacle"))
        {
            damagedTargets.Add(other.gameObject);
            Obstacle obstacleS = other.GetComponent<Obstacle>();
            obstacleS.photonView.RPC("OnObstacleHit", RpcTarget.All, monsterAIScript.damage);
        }
    }

}
