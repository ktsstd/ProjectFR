using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviourPun, IPunObservable
{
    public MonsterInfo monsterinfo;

    public float CurHp;
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public float attackTimer;
    public float attackSpeed;
    public float attackAnimationDelay;
    public float recognizedistance;
    public float monsterSlowCurTime;
    public float targetSearchTime = 0.5f;
    public float targetSearchTimer;

    public bool canMove;
    public bool isDie;

    public Transform target;

    public GameObject attackboundary;
    public GameObject sloweffect;

    //public string[] priTarget;

    public CapsuleCollider targetCollider;
    public Rigidbody rigid;
    public NavMeshAgent agent;
    public Animator animator;

    public List<(float slowtime, float slowmoveSpeed)> slowEffects = new List<(float, float)>();

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        agent.speed = monsterinfo.speed;
        damage = monsterinfo.damage;
        attackRange = monsterinfo.attackRange;
        attackCooldown = monsterinfo.attackCooldown;
        attackSpeed = monsterinfo.attackSpeed;
        attackAnimationDelay = monsterinfo.AttackAnimationDelay;
        recognizedistance = monsterinfo.redistance;
        attackTimer = monsterinfo.attackTimer;
        monsterSlowCurTime = 0;
        canMove = true;
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            CurHp = monsterinfo.health;
        }
        else
        {
            CurHp = monsterinfo.health / 2;
            damage = monsterinfo.damage / 2;
        }
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

        if (PhotonNetwork.IsMasterClient)
        {
            targetSearchTimer -= Time.deltaTime;
            if (targetSearchTimer <= 0f)
            {
                photonView.RPC("GetClosestTarget", RpcTarget.All);
                targetSearchTimer = targetSearchTime;
            }
        }
        else
        {

        }
        if (target == null) return;
        targetCollider = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = targetCollider.ClosestPoint(transform.position);
        if (canMove && CurHp > 0)
        {
            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance <= attackRange)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                animator.SetBool("Run", false);
                if (attackTimer <= 0)
                {
                    canMove = false;
                    Attack();
                }
                else
                {
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
                agent.SetDestination(target.position);
                animator.SetBool("Run", true);
            }
        }
        else
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("Run", false);
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }
    [PunRPC]
    public void GetClosestTarget()
    {
        float closestDistance = recognizedistance;
        Transform tempClosestTarget = null;

        foreach (string targetTag in monsterinfo.priTarget)
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
                    SummonAI summonAIS = possibleTarget.GetComponent<SummonAI>();
                    if (summonAIS != null && summonAIS.currentHp <= 0)
                        continue;
                }

                float distance = Vector3.Distance(transform.position, possibleTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    tempClosestTarget = possibleTarget.transform;
                }
            }
        }

        if (tempClosestTarget == null && !monsterinfo.isBoss)
        {
            GameObject objectTarget = GameObject.FindGameObjectWithTag("Object");
            if (objectTarget != null)
                tempClosestTarget = objectTarget.transform;
        }

        target = tempClosestTarget;
    }
    public virtual void Attack() // todo -> attacking animation
    {
        attackboundary.SetActive(true);
        Attackboundary attackboundaryScript = attackboundary.GetComponent<Attackboundary>();
        attackboundaryScript.Starting(attackSpeed, attackAnimationDelay);
    }

    public virtual void AttackAnimation()
    {
        animator.SetTrigger("StartAttack");
        Debug.Log("AttackAnimation Called");
    }

    public virtual void AttackSound() { }

    [PunRPC]
    public void AfterAttack()
    {
        attackTimer = attackCooldown;
        canMove = true;
    }

    Transform knockbackTransform;
    public virtual void OnMonsterKnockBack(Transform _transform)
    {
        knockbackTransform = _transform;
        if (CurHp <= 0) return;
        photonView.RPC("OnMonsterKnockBackStart", RpcTarget.All);
    }
    [PunRPC]
    public void OnMonsterKnockBackStart()
    {
        if (monsterinfo.isBoss) return;
        if (canMove)
        {
            canMove = false;
        }  
        agent.enabled = false;

        Vector3 vec = transform.position - knockbackTransform.position;
        vec.x = Mathf.Sign(vec.x) * 3f;
        vec.y = 0;
        vec.z = Mathf.Sign(vec.z) * 3f;
        rigid.AddForce(vec, ForceMode.Impulse);
        knockbackTransform = null;

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
        rigid.velocity = Vector3.zero;
        agent.enabled = true;
        if (attackboundary != null && !attackboundary.activeSelf)
        {
            canMove = true;
        }     
    }

    public virtual void OnMonsterSpeedDown(float _time, float _moveSpeed)
    {
        if (monsterSlowCurTime > 0 && !monsterinfo.isBoss)
        {
            slowEffects.Add((_time, _moveSpeed));
            slowEffects.Sort((a, b) => b.slowmoveSpeed.CompareTo(a.slowmoveSpeed));
        }
        else if (monsterSlowCurTime <= 0 && !monsterinfo.isBoss)
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
    public virtual void MonsterDmged(float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("OnMonsterHit", RpcTarget.All, damage);     
    }
    [PunRPC]
    public virtual void OnMonsterHit(float damage)
    {
        if (CurHp > 0)
        {
            CurHp -= damage;
            if (CurHp <= 0)
            {
                canMove = false;
                if (attackboundary != null && attackboundary.activeSelf)
                {
                    attackboundary.SetActive(false);
                }
                animator.SetTrigger("Die");
                Invoke("DestroyMonster", 1f);
            }
        }
    }
    public virtual void DestroyMonster()
    {
        PhotonNetwork.Destroy(gameObject);
        GameManager.Instance.CheckMonster();
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
