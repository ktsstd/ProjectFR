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
    private float monsterSlowCurTime;


    public Transform target;
    private Rigidbody rigidbody;
    private List<(float slowtime, float slowmoveSpeed)> slowEffects = new List<(float, float)>();

    public NavMeshAgent agent;
    public bool canMove = true;
    public Animator animator;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = 50;
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
            target = GetClosestTarget();

        if (target != null && canMove && agent.enabled)
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
        else if (!canMove && agent.enabled)
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
        Vector3 attackFowardPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z) + transform.forward * 1.5f;
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
            GameManager.Instance.CheckMonster();
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnMonsterKnockBack(Transform _transform)
    {
        canMove = false;
        agent.enabled = false;

        Vector3 vec = transform.position - _transform.position;
        vec.x = Mathf.Sign(vec.x) * 2;
        vec.y = 0;
        vec.z = Mathf.Sign(vec.z) * 2;
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
    IEnumerator MonsterStun(float _time)
    {
        yield return new WaitForSeconds(_time);
        rigidbody.velocity = Vector3.zero;
        agent.enabled = true;
        canMove = true;
    }

    public virtual void OnMonsterSpeedDown(float _time, float _moveSpeed) // HERE TS
    {
        if (monsterSlowCurTime > 0)
        {
            slowEffects.Add((_time, _moveSpeed));
            slowEffects.Sort((a, b) => b.slowmoveSpeed.CompareTo(a.slowmoveSpeed));
        }
        else
        {
            OnMonsterSpeedDownStart(_time, _moveSpeed);
        }
    }

    private Coroutine speedCoroutine;
    public virtual void OnMonsterSpeedDownStart(float _time, float _moveSpeed)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(MonsterSpeedDowning(_time, _moveSpeed));
    }

    IEnumerator MonsterSpeedDowning(float _time, float _moveSpeed)
    {
        monsterInfo.speed = _moveSpeed;
        while (monsterSlowCurTime > 0)
        {
            yield return null;

            for (int i = 0; i < slowEffects.Count; i++)
            {
                var remaineffect = slowEffects[i];
                slowEffects[i] = (remaineffect.slowtime - Time.deltaTime, remaineffect.slowmoveSpeed);
            }
        }
        // yield return new WaitForSeconds(_time);
        if (slowEffects.Count > 0)
        {
            var nextSlowEffect = slowEffects[0];
            slowEffects.RemoveAt(0);
            monsterInfo.speed += _moveSpeed;
            OnMonsterSpeedDownStart(nextSlowEffect.slowtime, nextSlowEffect.slowmoveSpeed);
        }
        else
        {
            speedCoroutine = null;
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
