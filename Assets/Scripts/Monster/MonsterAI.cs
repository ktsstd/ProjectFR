using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
{
    public MonsterInfo monsterInfo;

    public Transform target;

    private NavMeshAgent agent;
    public bool canMove = true;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = monsterInfo.speed;    
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
    }
    protected virtual void Update()
    {
        target = GetClosestTarget();

        if (target != null && canMove)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= monsterInfo.attackRange)
            {
                if (monsterInfo.attackTimer <= 0)
                {
                    Attack();
                    canMove = false;
                }
                else
                { 
                    agent.ResetPath(); // todo -> Idle animation
                }
            }
            else
            {
                agent.SetDestination(target.position); // todo -> moving animation
            }

            if (monsterInfo.attackTimer > 0)
            {
                monsterInfo.attackTimer -= Time.deltaTime;
            }
            Vector3 directionToTarget = target.position - transform.position;
            if (directionToTarget != Vector3.zero) 
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
        else if (!canMove)
        {
            agent.ResetPath(); // todo -> Idle animation
            target = GetClosestTarget();
        }    
    }
    protected Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (string targetTag in monsterInfo.priTarget)
        {
            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetTag);

            if (possibleTargets.Length == 0 || possibleTargets == null) continue;

            foreach (GameObject possibleTarget in possibleTargets)
            {
                float distance = Vector3.Distance(transform.position, possibleTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = possibleTarget.transform;
                }
            }
        }

        return closestTarget;
    }
    protected virtual void Attack() // todo -> attacking animation
    {
        string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[0].name;
        // Vector3 directionToTarget = GetClosestTarget().position - transform.position;
        // Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        // transform.rotation = Quaternion.Slerp(transform.localRotation, lookRotation, 80);
        Vector3 attackFowardPos = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1;
        GameObject AttackObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos, Quaternion.identity);
        AttackObj.transform.SetParent(this.transform);
    }

    public virtual void AttackSuccess(GameObject Obj)
    {
        Debug.Log("attack success: " + Obj);
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        canMove = true;
    } 

    protected virtual void MonsterDmged(float damage)
    {
        if (!photonView.IsMine) return;

        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(monsterInfo.health);
            stream.SendNext(monsterInfo.attackTimer);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            monsterInfo.health = (float)stream.ReceiveNext();
            monsterInfo.attackTimer = (float)stream.ReceiveNext();
        }
    }
}
