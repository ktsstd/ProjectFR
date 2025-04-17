using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
{
    public MonsterInfo monsterInfoScript;
    public MonsterInfo monsterInfo;
    public float monsterSlowCurTime;
    public float CurHp;
    public GameObject sloweffect;
    public GameObject AttackBoundary;


    public Transform target;
    private Rigidbody rigidbody;
    private List<(float slowtime, float slowmoveSpeed)> slowEffects = new List<(float, float)>();

    public NavMeshAgent agent;
    public bool canMove = true;
    public Animator animator;
    Transform closestTarget;

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = 50;
        monsterInfo = Instantiate(monsterInfoScript);
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            CurHp = monsterInfo.health;
        }
        else
        {
            CurHp = monsterInfo.health / 2; 
            monsterInfo.damage = monsterInfo.damage / 2;
        }
        
        agent.speed = monsterInfo.speed;
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        monsterSlowCurTime = 0;
    }
    public virtual void Update()
    {
        if (sloweffect != null)
        {
            ParticleSystem ps = sloweffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            }
        }
        target = GetClosestTarget();
        if (target == null) return;
        CapsuleCollider coll = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = coll.ClosestPoint(transform.position);
        if (target != null && canMove)
        {
            float distance = Vector3.Distance(transform.position, targetPos);
            if (animator != null)
                animator.SetBool("Run", true);

            if (distance <= monsterInfo.attackRange)
            {
                agent.velocity = Vector3.zero;
                if (animator != null)
                    animator.SetBool("Run", false);
                if (monsterInfo.attackTimer <= 0)
                {
                    Attack();
                    canMove = false;
                }
                else
                {
                    agent.ResetPath(); // todo -> Idle animation
                    Vector3 directionToTarget = target.position - transform.position;
                    if (directionToTarget != Vector3.zero)
                    {
                        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
                    }
                }
            }
            else
            {
                agent.SetDestination(target.position); // todo -> moving animation
                if (animator != null)
                    animator.SetBool("Run", true);
            }
        }
        else if (!canMove && agent.enabled)
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath(); // todo -> Idle animation
            if (animator != null)
                animator.SetBool("Run", false);
            target = GetClosestTarget();
        }

        if (monsterInfo.attackTimer > 0)
        {
            monsterInfo.attackTimer -= Time.deltaTime;
        }
    }
    protected Transform GetClosestTarget()
    {
        float closestDistance = monsterInfo.redistance;
        Transform tempClosestTarget = null;

        foreach (string targetTag in monsterInfo.priTarget)
        {
            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetTag);
            if (possibleTargets == null || possibleTargets.Length == 0)
                continue;

            foreach (GameObject possibleTarget in possibleTargets)
            {
                if (possibleTarget.CompareTag("Player"))
                {
                    PlayerController playerCtrl = possibleTarget.GetComponent<PlayerController>();
                    if (playerCtrl != null && playerCtrl.playerHp <= 0)
                        continue;
                }
                else if (possibleTarget.CompareTag("Summon"))
                {

                }

                    float distance = Vector3.Distance(transform.position, possibleTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    tempClosestTarget = possibleTarget.transform;
                }
            }
        }

        if (tempClosestTarget == null && !monsterInfo.isBoss)
        {
            GameObject objectTarget = GameObject.FindGameObjectWithTag("Object");
            if (objectTarget != null)
                tempClosestTarget = objectTarget.transform;
        }

        return tempClosestTarget;
    }


    public virtual void Attack() // todo -> attacking animation
    {
        if (animator != null)
            animator.SetTrigger("StartAttack");
        AttackBoundary.SetActive(true);
        Attackboundary attackboundaryScript = AttackBoundary.GetComponent<Attackboundary>();
        attackboundaryScript.Starting();
    }

    public virtual void AttackSuccess(GameObject Obj, int damage)
    {
        Debug.Log("attack success: " + Obj + ":" + damage);
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        canMove = true;
    }
    public virtual void MonsterDmged(float damage)
    {
        if (CurHp > 0)
        {
            CurHp -= damage;
        }
        else if (CurHp <= 0 && canMove)
        {
            canMove = false;
            if (AttackBoundary != null)
            {
                AttackBoundary.SetActive(false);
            }
            if (animator != null)
                animator.SetTrigger("Die");
            Invoke("DestroyMonster", 1f);
        }
    }
    public virtual void DestroyMonster()
    {
        PhotonNetwork.Destroy(gameObject);
        GameManager.Instance.CheckMonster();
    }
    [PunRPC]
    public void OnMonsterKnockBack(Transform _transform)
    {
        if (monsterInfo.isBoss) return;
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

    public virtual void OnMonsterSpeedDown(float _time, float _moveSpeed)
    {
        if (monsterSlowCurTime > 0 && !monsterInfo.isBoss)
        {
            slowEffects.Add((_time, _moveSpeed));
            slowEffects.Sort((a, b) => b.slowmoveSpeed.CompareTo(a.slowmoveSpeed));
        }
        else if (monsterSlowCurTime <= 0 && !monsterInfo.isBoss)
        {
            OnMonsterSpeedDownStart(_time, _moveSpeed);
        }
    }

    private Coroutine speedCoroutine;
    public void OnMonsterSpeedDownStart(float _time, float _moveSpeed)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(MonsterSpeedDowning(_time, _moveSpeed));
    }

    IEnumerator MonsterSpeedDowning(float _time, float _moveSpeed)
    {
        agent.speed -= _moveSpeed;
        sloweffect.SetActive(true);
        monsterSlowCurTime = _time;
        while (monsterSlowCurTime > 0)
        {
            yield return null;
            monsterSlowCurTime -= Time.deltaTime;

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
            agent.speed += _moveSpeed;
            OnMonsterSpeedDownStart(nextSlowEffect.slowtime, nextSlowEffect.slowmoveSpeed);
        }
        else
        {
            speedCoroutine = null;
            agent.speed += _moveSpeed;
            sloweffect.SetActive(false);
        }
    }
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(CurHp);
            stream.SendNext(canMove);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            CurHp = (float)stream.ReceiveNext();
            canMove = (bool)stream.ReceiveNext();
        }
    }
}
