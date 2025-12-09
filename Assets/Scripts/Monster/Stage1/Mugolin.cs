using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Mugolin : MonsterAI
{
    [SerializeField] private GameObject MugolinEffect;
    [SerializeField] private AudioSource audioS;
    private bool IsRolling;

    private float defaultspeed;
    private float Increasespeed;
    private float IncreasePerspeed;

    public override void Awake()
    {
        base.Awake();
        defaultspeed = agent.speed;
        Increasespeed = defaultspeed * 2;
        IncreasePerspeed = (Increasespeed - defaultspeed) / 5;
        StartCoroutine(StartMove());
    }
    public override void Update()
    {
        base.Update();
        MugolinEffect.SetActive(IsRolling);
        animator.SetBool("isRolling", IsRolling);
        if (IsRolling && isMoving)
        {
            audioS.volume = SoundManager.Instance.SfxMonsterVolume / 3;
            if (!audioS.isPlaying)
                audioS.Play();
        }
        else if (!IsRolling && isMoving)
        {
            audioS.Stop();
        }
    }

    public override void Move()
    {
        if (target == null) return;
        isMoving = true;
        targetCollider = target.GetComponent<Collider>();
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
            if (distance <= skillRange[i] && skillTimer[i] <= 0f && currentState == States.Idle && thinkTimer <= 0f && !IsRolling)
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
                agent.SetDestination(targetPos);
                animator.SetBool("Run", true);
            }
        }
        if (distance <= attackRange && currentState != States.Attack && !IsRolling)
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
            agent.SetDestination(targetPos);
            animator.SetBool("Run", true);
        }
    }

    private IEnumerator StartMove()
    {
        IsRolling = true;
        for (int i = 0; i < 3; i++)
        {
            agent.speed += IncreasePerspeed;
            yield return new WaitForSeconds(1f);
        }
        IsRolling = false;
        SpeedReset();
        yield break;
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player" && IsRolling)
        {
            agent.ResetPath();
            IsRolling = false;
            currentState = States.Stun;
            animator.SetTrigger("Stun");
            agent.velocity = Vector3.zero;
            PlayerController playerctrl = other.gameObject.GetComponent<PlayerController>();
            playerctrl.photonView.RPC("OnPlayerStun", RpcTarget.All, 1f);
            playerctrl.photonView.RPC("OnPlayerHit", RpcTarget.All, 150 + (0.8f * damage));
            agent.velocity = Vector3.zero;
            StartCoroutine(SpeedReset());
        }
        else if (other.gameObject.tag == "Object" && IsRolling)
        {
            agent.ResetPath();
            IsRolling = false;
            currentState = States.Stun;
            agent.velocity = Vector3.zero;
            animator.SetTrigger("Stun");
            Object objectS = other.gameObject.GetComponent<Object>();
            objectS.Damaged((150 + (0.8f * damage)) + (120 + (0.8f * damage)));
            StartCoroutine(SpeedReset());
        }
        else if (other.gameObject.tag == "Summon" && IsRolling)
        {
            agent.ResetPath();
            IsRolling = false;
            currentState = States.Stun;
            agent.velocity = Vector3.zero;
            animator.SetTrigger("Stun");
            SummonAI summonS = other.gameObject.GetComponent<SummonAI>();
            summonS.photonView.RPC("OnSummonHit", RpcTarget.All, 150 + (0.8f * damage));
            StartCoroutine(SpeedReset());
        }
        else if (other.gameObject.tag == "Obstacle" && IsRolling)
        {
            agent.ResetPath();
            IsRolling = false;
            currentState = States.Stun;
            agent.velocity = Vector3.zero;
            animator.SetTrigger("Stun");
            Obstacle obstacleS = other.gameObject.GetComponent<Obstacle>();
            obstacleS.photonView.RPC("OnObstacleHit", RpcTarget.All, (150 + (0.8f * damage)) + (120 + (0.8f * damage)));
            StartCoroutine(SpeedReset());
        }
    }

    IEnumerator SpeedReset()
    {
        if (monsterSlowCurTime > 0)
        {
            float slowSpeed = Increasespeed - agent.speed;
            agent.speed = defaultspeed - slowSpeed;
        }
        else
        {
            agent.speed = defaultspeed;
        }
        yield return new WaitForSeconds(1.333f);
        currentState = States.Idle;
        attackTimer = attackCooldown;
    }
    public override void AttackEvent()
    {
        if (currentState == States.Attack)
        {
            if (target.tag == "Object")
            {
                Object ObjectS = target.GetComponent<Object>();
                SoundManager.Instance.PlayMonsterSfx(0, transform.position);
                ObjectS.Damaged(damage);
                attackTimer = attackCooldown;
                currentState = States.Idle;
            }
            else if (target.tag == "Obstacle")
            {
                Obstacle obstacleS = target.GetComponent<Obstacle>();
                SoundManager.Instance.PlayMonsterSfx(0, transform.position);
                obstacleS.photonView.RPC("OnObstacleHit", RpcTarget.All, damage);
                attackTimer = attackCooldown;
                currentState = States.Idle;
            }
            else if (target.tag == null)
            {
                attackTimer = attackCooldown;
                currentState = States.Idle;
            }
        }
        else
        {
            return;
        }
    }
}
