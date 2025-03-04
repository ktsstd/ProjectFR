using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[CreateAssetMenu(fileName = "MonsterInfo", menuName = "Monster")]
public class MonsterInfo : ScriptableObject
{
    public float speed;
    public float damage;
    public float health; 

    public float attackRange; 
    public float attackSpeed; 
    public float attackCooldown; 
    public float attackTimer; 

    public string[] priTarget;
}

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
{
    public MonsterInfo monsterInfo;

    public Transform target;

    private NavMeshAgent agent;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
    }
    protected void Update()
    {
        target = GetClosestTarget();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= monsterInfo.attackRange)
            {
                if (monsterInfo.attackTimer <= 0)
                {
                    Attack();
                }
                else
                {
                    monsterInfo.attackTimer -= Time.deltaTime;
                    agent.ResetPath();
                }
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

    protected Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (string targetTag in monsterInfo.priTarget)
        {
            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetTag);

            if (possibleTargets.Length == 0) continue;

            foreach (GameObject possibleTarget in possibleTargets)
            {
                float distance = Vector3.Distance(transform.position, possibleTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = possibleTarget.transform;
                }
            }

            if (closestTarget != null) break;
        }

        return closestTarget;
    }

    protected virtual void Attack()
    {
        // normal attack
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
    }
    protected void MonsterDmged(float damage)
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
