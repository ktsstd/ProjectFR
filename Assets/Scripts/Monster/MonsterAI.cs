using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
{
    public MonsterInfo monsterInfoScript;
    public MonsterInfo monsterInfo;
    private Transform playerController;

    public Transform target;
    private Rigidbody rigidbody;

    public NavMeshAgent agent;
    public bool canMove = true;
    public bool knockback = false;
    public Animator animator;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterInfo = Instantiate(monsterInfoScript);
        agent.speed = monsterInfo.speed;
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            OnMonsterKnockBack(playerController);
        if (agent == null) return;
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
        else if (!canMove && !knockback)
        {
            agent.ResetPath(); // todo -> Idle animation
            target = GetClosestTarget();
        }
    }
    protected Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = monsterInfo.redistance;

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
                else
                {
                    if (!monsterInfo.isBoss)
                    {
                        GameObject objectTarget = GameObject.FindGameObjectWithTag("TestObject");
                        return objectTarget.transform;
                    }
                    else
                    {
                        GameObject objectTarget = GameObject.FindGameObjectWithTag("monsterInfo.priTarget[0]");
                        return objectTarget.transform;
                    }
                }
            }
        }

        return closestTarget;
    }
    protected virtual void Attack() // todo -> attacking animation
    {
        string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[0].name;
        Vector3 attackFowardPos = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1.5f;
        animator.SetTrigger("StartAttack");
        GameObject AttackObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos, Quaternion.identity);
        AttackObj.transform.SetParent(this.transform);
    }

    public virtual void AttackSuccess(GameObject Obj, int damage)
    {
        Debug.Log("attack success: " + Obj + ":" + damage);
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

    private void OnMonsterKnockBack(Transform _transform)
    {
        Vector3 vec = transform.position - _transform.position;
        vec.x = Mathf.Sign(vec.x) * 5;
        vec.z = Mathf.Sign(vec.z) * 5;
        rigidbody.AddForce(vec, ForceMode.Impulse);
        OnMonsterStun(0.5f);
    }

    private Coroutine stunCoroutine;
    public void OnMonsterStun(float _time)
    {
        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);

        stunCoroutine = StartCoroutine(MonsterStun(_time));
    }

    IEnumerator MonsterStun(float _time) // 스턴 상태 처리
    {
        canMove = false;
        knockback = true;
        agent.enabled = false;
        yield return new WaitForSeconds(_time);
        agent.enabled = true;
        canMove = true;
        knockback = false ;
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
