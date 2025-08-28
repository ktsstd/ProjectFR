using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviourPunCallbacks
{
    public MonsterInfo monsterInfo;
    public Animator animator;
    public Rigidbody rigid;
    public Transform target;
    public NavMeshAgent agent;
    public CapsuleCollider targetCollider;

    public GameObject StunningEffect;
    public GameObject sloweffect;

    public bool isMoving = false;

    public float CurHp; // 체력
    public float damage; // 공격력
    public float attackRange; // 사거리
    public float attackCooldown; // 공속
    public float attackTimer; // 공속 타이머
    public float recognizedistance; // 인지 범위
    public float monsterSlowCurTime; // 구속 현재 시간
    public float targetSearchTime = 2f; // 인지 재탐색 시간
    public float targetSearchTimer; // 인지 타이머

    public List<(float slowtime, float slowmoveSpeed)> slowEffects = new List<(float, float)>();

    public States currentState;

    public enum States
    {
        Idle,
        Stop,
        Attack,
        Dash,
        Stun,
        Die
    }
    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        currentState = States.Idle;
        CurHp = monsterInfo.health;
        damage = monsterInfo.damage;
        attackRange = monsterInfo.attackRange;
        attackCooldown = monsterInfo.attackCooldown;
        attackTimer = attackCooldown;
        recognizedistance = monsterInfo.redistance;
        targetSearchTimer = targetSearchTime;
        agent.speed = monsterInfo.speed;

        isMoving = false;
    }
    public virtual void Update()
    {        
        if (PhotonNetwork.IsMasterClient)
        {
            switch (currentState)
            {
                case States.Idle:
                    photonView.RPC("Move", RpcTarget.AllBuffered);
                    break;
                case States.Stop:
                    break;
                case States.Attack:
                    break;
                case States.Die:
                    break;
                case States.Stun:
                    break;
            }

            if (targetSearchTimer <= 0f)
            {
                photonView.RPC("RecognizePlayer", RpcTarget.AllBuffered);
                targetSearchTimer = targetSearchTime;
            }
        }
        targetSearchTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }

    [PunRPC]
    public virtual void Move()
    {
        if (target == null) return;
        isMoving = true;
        targetCollider = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = targetCollider.ClosestPoint(transform.position);
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance <= attackRange)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("Run", false);
            if (attackTimer <= 0 && currentState != States.Attack)
            {
                currentState = States.Attack;
                isMoving = false;                
                Attack();
            }
            else
            {
                Vector3 directionToTarget = target.position - transform.position;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 30f);
                }
            }
        }
        else
        {
            agent.SetDestination(target.position);
            animator.SetBool("Run", true);
        }
    }

    public virtual void Attack() 
    {
        animator.SetTrigger("StartAttack");
    }

    public virtual void AttackEvent() { }

    public virtual void AttackEffect() { }

    public virtual void AttackSound() { }
    public virtual void AttackAnimation() { }

    public virtual void OnMonsterKnockBack(Transform _transform) { } // 진짜 하기도싫고 생각하기도 귀찮다

    [PunRPC]
    public void StunEffect()
    {
        StunningEffect.SetActive(true);
        animator.SetTrigger("Stun");
        isMoving = false;
    }
    private Coroutine stunCoroutine;
    public void OnMonsterStun(float _time)
    {
        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);
        currentState = States.Stun;

        stunCoroutine = StartCoroutine(MonsterStun(_time));
    }

    public virtual IEnumerator MonsterStun(float _time)
    {
        photonView.RPC("StunEffect", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(_time);
        rigid.velocity = Vector3.zero;
        currentState = States.Idle;
        // 공격 정지
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

    [PunRPC]
    public void RecognizePlayer()
    {
        float closestDistance = recognizedistance;
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

        if (tempClosestTarget == null && !monsterInfo.isBoss)
        {
            GameObject objectTarget = GameObject.FindGameObjectWithTag("Object");
            if (objectTarget != null)
                tempClosestTarget = objectTarget.transform;
        }

        target = tempClosestTarget;
    }

    public virtual void MonsterDmged(float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("OnMonsterHit", RpcTarget.All, damage);
    }
    [PunRPC]
    public virtual void OnMonsterHit(float damage)
    {
        if (CurHp > 0 && currentState != States.Die)
        {
            CurHp -= damage;
            if (CurHp <= 0)
            {
                currentState = States.Die;
                agent.ResetPath();
                // 공격 취소
                animator.SetTrigger("Die");
                Invoke("DestroyMonster", 1.5f);
            }
        }
    }
    public virtual void DestroyMonster()
    {
        PhotonNetwork.Destroy(gameObject);
        if (SceneManagerHelper.ActiveSceneName == "Tutorial")
        {
            TutorialManagement.Instance.CheckMonster();
        }
        else
        {
            GameManager.Instance.CheckMonster();
        }
    }
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
