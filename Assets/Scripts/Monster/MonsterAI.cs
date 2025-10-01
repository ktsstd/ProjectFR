using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
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
    public float attackDelay; // 공격 딜레이
    public float[] skillRange; // 스킬 사거리 
    public float[] skillCooldown; // 스킬 쿨타임
    public float[] skillTimer; // 스킬 타이머
    public float[] skillDelay; // 스킬 딜레이
    public float thinkTime = 3f; // 스킬 선택 쿨타임
    public float thinkTimer; // 스킬 선택 타이머
    public float attackTimer; // 공속 타이머
    public float recognizedistance; // 인지 범위
    public float monsterSlowCurTime; // 구속 현재 시간
    public float targetSearchTime = 4f; // 인지 재탐색 시간
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
        if (PhotonNetwork.PlayerList.Length > 2)
        {
            CurHp = monsterInfo.health;
            damage = monsterInfo.damage;
        }
        else if (PhotonNetwork.PlayerList.Length == 2)
        {
            CurHp = (monsterInfo.health * 0.6f);
            damage = (monsterInfo.damage * 0.6f);
        }
        else if (PhotonNetwork.PlayerList.Length == 1)
        {
            CurHp = monsterInfo.health * 0.5f;
            damage = monsterInfo.damage * 0.5f;
        }
        skillCooldown = new float[monsterInfo.skillCooldown.Length];
        skillTimer = new float[monsterInfo.skillCooldown.Length];
        skillRange = new float[monsterInfo.skillCooldown.Length];
        for (int i = 0; i < monsterInfo.skillCooldown.Length; i++)
        {
            skillCooldown[i] = monsterInfo.skillCooldown[i];
            skillTimer[i] = skillCooldown[i];
            skillRange[i] = monsterInfo.skillRange[i];
            //skillDelay[i] = monsterInfo.skillDelay[i];
        }
        Array.Sort(skillRange);
        attackRange = monsterInfo.attackRange;
        attackCooldown = monsterInfo.attackCooldown;
        attackTimer = attackCooldown;
        attackDelay = monsterInfo.attackdDelay;
        thinkTimer = thinkTime;
        recognizedistance = monsterInfo.redistance;
        targetSearchTimer = targetSearchTime;
        agent.speed = monsterInfo.speed;

        isMoving = false;
    }
    public virtual void Update()
    {
        //if (!PhotonNetwork.IsMasterClient) return;
        switch (currentState)
        {
            case States.Idle:
                Move();
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
        if (targetSearchTimer <= 0f && PhotonNetwork.IsMasterClient)
        {
            //photonView.RPC("RecognizePlayer", RpcTarget.AllBuffered);
            RecognizePlayer();
        }
        targetSearchTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        thinkTimer -= Time.deltaTime;
        for (int i = 0; i < skillTimer.Length; i++)
        {
            if (skillTimer[i] > 0f)
            {
                skillTimer[i] -= Time.deltaTime;
            }
        }
    }

    //[PunRPC]
    public virtual void Move()
    {
        if (target == null) return;
        isMoving = true;
        targetCollider = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = targetCollider.ClosestPoint(transform.position);
        float distance = Vector3.Distance(transform.position, targetPos);
        if (currentState == States.Idle)
        {
            Vector3 directionToTarget = target.position - transform.position;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 6f);
            }
        }
        for (int i = 0; i < skillRange.Length; i++)
        {
            if (distance <= skillRange[i] && skillTimer[i] <= 0f && currentState == States.Idle && thinkTimer <= 0f)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                animator.SetBool("Run", false);
                agent.velocity = Vector3.zero;
                currentState = States.Attack;
                isMoving = false;
                int randomSkill = GetRandomSkill(skillRange[i]);
                if (randomSkill != -1)
                {
                    SkillAttack(randomSkill);
                }
                break;
            }
            else if (distance > skillRange[i] && skillTimer[i] <= 0f && currentState == States.Idle && thinkTimer <= 0f)
            {
                agent.SetDestination(target.position);
                animator.SetBool("Run", true);
            }
        }
        if (distance <= attackRange && currentState != States.Attack)
        {
            agent.ResetPath();
            animator.SetBool("Run", false);
            if (attackTimer <= 0 && currentState != States.Die)
            {
                currentState = States.Attack;
                isMoving = false;                
                AttackStart();
            }
        }
        else if (distance > attackRange && currentState != States.Attack)
        {
            agent.SetDestination(target.position);
            animator.SetBool("Run", true);
        }
    }

    public virtual void AttackStart()
    {
        Invoke("Attack", attackDelay);
    }

    public virtual void Attack() 
    {
        animator.SetTrigger("StartAttack");
    }

    public virtual void SkillAttack(int skillIndex)
    {
        if (currentState == States.Die) return;
        skillTimer[skillIndex] = skillCooldown[skillIndex];
        thinkTimer = thinkTime;
        currentState = States.Idle;
    }

    public virtual int GetRandomSkill(float availSkillRange)
    {
        List<int> availableSkills = new List<int>();

        for (int i = 0; i < skillTimer.Length; i++)
        {
            if (skillTimer[i] <= 0f && skillRange[i] == availSkillRange)
            {
                availableSkills.Add(i);
            }
        }

        if (availableSkills.Count > 0)
        {
            return availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
        }
        else
        {
            thinkTimer = thinkTime;
            currentState = States.Idle;
        }
        return -1;
    }

    public virtual void AttackEvent() { }

    public virtual void ShowAttackBoundary() { }

    public virtual void AttackEffect() { }

    public virtual void AttackSound() { }
    public virtual void AttackAnimation() { }

    public virtual void OnMonsterKnockBack(Transform _transform) 
    {
        agent.ResetPath();
        agent.enabled = false;
        currentState = States.Stun;
        Vector3 knockbackDirection = (transform.position - _transform.position).normalized;
        rigid.AddForce(knockbackDirection * 8f, ForceMode.Impulse);
        Invoke("ResetState", 0.6f);
    } 
    public void ResetState()
    {
        if (currentState != States.Die)
        {
            agent.enabled = true;
            currentState = States.Idle;            
        }
    }

    [PunRPC]
    public void StunEffect(float _time)
    {
        StunningEffect.SetActive(true);
        animator.SetTrigger("Stun");
        isMoving = false;
        Invoke("StopStunEffect", _time);
    }
    public void StopStunEffect()
    {
        StunningEffect.SetActive(false);
    }
    private Coroutine stunCoroutine;
    public virtual void OnMonsterStun(float _time)
    {
        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);
        currentState = States.Stun;

        stunCoroutine = StartCoroutine(MonsterStun(_time));
    }

    public virtual IEnumerator MonsterStun(float _time)
    {
        photonView.RPC("StunEffect", RpcTarget.AllBuffered, _time);
        yield return new WaitForSeconds(_time);
        rigid.velocity = Vector3.zero;
        if (currentState != States.Die)
        {
            currentState = States.Idle;
        }        
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

    //[PunRPC]
    public void RecognizePlayer()
    {
        targetSearchTimer = targetSearchTime;
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
        photonView.RPC("Settarget", RpcTarget.AllBuffered, target.name);
        //targetSearchTimer = targetSearchTime;

    }
    [PunRPC]
    public void Settarget(string targetname)
    {
        target = GameObject.Find(targetname).transform;
    }

    public virtual void MonsterDmged(float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("OnMonsterHit", RpcTarget.All, damage);
    }
    [PunRPC]
    public virtual void OnMonsterHit(float damage)
    {
        CurHp -= damage;
        if (CurHp <= 0 && currentState != States.Die)
        {
            currentState = States.Die;
            if (agent != null)
            {
                agent.ResetPath();
                agent.enabled = false;
            }
            currentState = States.Die;
            // 공격 취소
            animator.SetTrigger("Die");
            currentState = States.Die;
            Invoke("DestroyMonster", 1.5f);
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
