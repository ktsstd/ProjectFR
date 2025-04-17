using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonAI : MonoBehaviourPunCallbacks
{
    public float currentHp;
    public float MaxHp;
    public float Atk;
    public float speed;
    public float summonTime;

    NavMeshAgent agent;

    public enum States // idle, attack, die
    {
        Idle,
        Move,
        Attack,
        Die
    }
    public States currentStates;

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentStates = States.Idle;
        Invoke("SelfDestroy", summonTime);
        StartCoroutine("SummonAICoroutine");
    }

    private void Update()
    {
        switch (currentStates)
        {
            case States.Idle:
                break;
            case States.Move:
                break;
            case States.Attack:
                break;
            case States.Die:
                break;
        }

    }

    Vector3 targetPos;
    IEnumerator SummonAICoroutine()
    {
        yield return new WaitForSeconds(2f);
        while (currentStates != States.Die)
        {
            targetPos = GetTargetPos(2f);
            transform.rotation = Quaternion.LookRotation(targetPos);
            agent.destination = targetPos;
            yield return new WaitForSeconds(1f);
        }
    }

    public Vector3 GetTargetPos(float _attackRange)
    {
        GameObject closeMonster = null;
        float min = 9999;

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (targets.Length == 0) return Vector3.zero;
        else
        {
            foreach (GameObject target in targets)
            {
                float targetDistance = Vector3.Distance(transform.position, target.transform.position);

                if (targetDistance < min)
                {
                    min = targetDistance;
                    closeMonster = target;
                }
            }
        }

        Vector3 direction = transform.position - closeMonster.transform.position;
        float distance = direction.magnitude;

        direction = direction.normalized;
        return closeMonster.transform.position + direction * _attackRange;
    }

    public void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}