using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonAI : MonoBehaviourPunCallbacks
{
    public float currentHp;
    public float maxHp;
    public float atk;
    public float attackRange;
    public float speed;
    public float summonTime;
    bool isDie;
    bool isRun;
    Coroutine summonCoroutine;

    NavMeshAgent agent;
    Rigidbody rigid;
    public Animator animator;
    public PhotonView pv;


    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        agent.speed = speed;
        isDie = false;

        summonCoroutine = StartCoroutine("SummonAICoroutine");
        Invoke("SelfDestroy", summonTime);
    }

    public void Update()
    {
        if (damageDelayTime > 0)
            damageDelayTime -= Time.deltaTime;

        if (targetPos != null && targetPos != Vector3.zero)
        {
            if (Vector3.Distance(targetPos, transform.position) <= 0.5f)
                isRun = false;
            else
                isRun = true;

            animator.SetBool("isRun", isRun);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            OnSummonHit(1000f);
        }
    }

    float damageDelayTime = 0.2f;
    [PunRPC]
    public void OnSummonHit(float _damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isDie)
            {
                float currentDamage = _damage;
                if (damageDelayTime <= 0)
                {
                    currentDamage = Shield(currentDamage);

                    currentHp -= currentDamage;
                    damageDelayTime = 0.2f;
                    Debug.Log(currentDamage);
                }

                if (currentHp <= 0)
                {
                    currentHp = 0;
                    isDie = true;
                    StopCoroutine(summonCoroutine);
                    animator.SetTrigger("Die");
                    Invoke("SelfDestroy", 3f);
                }
            }
        }
    }

    public virtual float Shield(float _damage) { return _damage; }

    Vector3 targetPos;
    IEnumerator SummonAICoroutine()
    {
        yield return new WaitForSeconds(2f);
        while (!isDie)
        {
            targetPos = GetTargetPos(attackRange);
            if (targetPos == Vector3.zero)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            agent.SetDestination(targetPos);
            yield return new WaitForSeconds(0.5f);
            if (targetMonster != null)
            {
                if (Vector3.Distance(transform.position, targetMonster.transform.position) <= attackRange + 1f)
                {
                    transform.rotation = Quaternion.LookRotation(targetMonster.transform.position - transform.position);
                    animator.SetTrigger("AttackAni");
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }

    public virtual void AttackAnimation() { }

    public GameObject targetMonster = null;
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
        targetMonster = closeMonster;

        Vector3 direction = transform.position - closeMonster.transform.position;
        float distance = direction.magnitude;

        direction = direction.normalized;
        return closeMonster.transform.position + direction * _attackRange;
    }

    [PunRPC]
    public void PlayTriggerAnimation(string _name)
    {
        animator.SetTrigger(_name);
    }

    public void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}