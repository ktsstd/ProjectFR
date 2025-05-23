using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mugolin : MonsterAI
{
    //private bool InSafeZone;
    [SerializeField] private GameObject MugolinEffect;
    [SerializeField] private GameObject StunEffect;
    [SerializeField] private AudioSource audioS;
    //[SerializeField] private GameObject RunEffect;
    private bool IsRolling;

    private float defaultspeed;
    private float Increasespeed;
    private float IncreasePerspeed;
    public override void Start()
    {
        base.Start();
        //InSafeZone = false;
        attackTimer = 99999f;
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
        if (IsRolling && canMove)
        {
            audioS.volume = SoundManager.Instance.SfxMonsterVolume / 3;
            if (!audioS.isPlaying)
                audioS.Play();
        }
        else if (!IsRolling && canMove)
        {
            StartCoroutine(RunEffectStart());
            audioS.Stop();
        }
    }

    IEnumerator RunEffectStart()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public override void Attack() // todo -> attacking animation
    {
        if (animator != null)
            animator.SetTrigger("StartAttack");
        Invoke("DamageObj", 0.767f);
    }

    public void DamageObj()
    {
        GameObject ObjectObj = GameObject.FindGameObjectWithTag("Object");
        Object ObjectS = ObjectObj.GetComponent<Object>();
        SoundManager.Instance.PlayMonsterSfx(0, transform.position); // Edit
        ObjectS.Damaged(damage);
        attackTimer = attackCooldown;
        canMove = true;
    }

    private IEnumerator StartMove() // todo -> speedup, rolling animationStart, speedup -> damageup
    {
        IsRolling = true;
        for (int i = 0; i < 5; i++)
        {
            agent.speed += IncreasePerspeed;
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }

    private IEnumerator StopMove()
    {
        StopCoroutine(StartMove()); // todo -> stun Effect
        animator.SetTrigger("Stun");
        StunEffect.SetActive(true);
        canMove = false;
        IsRolling = false;
        agent.velocity = Vector3.zero;        
        if (monsterSlowCurTime > 0)
        {
            float slowSpeed = Increasespeed - agent.speed;
            agent.speed = defaultspeed - slowSpeed;
        }
        else
        {
            agent.speed = defaultspeed;
        }
        yield return new WaitForSeconds(1.333f); // todo stun duration
        canMove = true;
        StunEffect.SetActive(false);
        StartCoroutine(StartMove());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && IsRolling)
        {
            StopCoroutine(StartMove());
            StartCoroutine(StopMove());
            PlayerController playerctrl = other.gameObject.GetComponent<PlayerController>();
            playerctrl.photonView.RPC("OnPlayerStun", RpcTarget.All, 1f);
            agent.velocity = Vector3.zero;
        }
    }

    public void StandUp()
    {
        if (!IsRolling) return;
        IsRolling = false;
        canMove = false;
        animator.SetTrigger("isUp");
        agent.velocity = Vector3.zero;
        attackTimer = attackCooldown;
        if (monsterSlowCurTime > 0)
        {
            float slowSpeed = Increasespeed - agent.speed;
            agent.speed = defaultspeed - slowSpeed;
        }
        else
        {
            agent.speed = defaultspeed;
        }
        StartCoroutine(Standing());
    }

    private IEnumerator Standing()
    {
        yield return new WaitForSeconds(0.6f);
        canMove = true;
    }
    public override IEnumerator MonsterStun(float _time)
    {
        yield return new WaitForSeconds(_time);
        rigid.velocity = Vector3.zero;
        agent.enabled = true;
        canMove = true;
    }
}
