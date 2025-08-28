using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mugolin : MonsterAI
{
    //private bool InSafeZone;
    [SerializeField] private GameObject MugolinEffect;
    [SerializeField] private AudioSource audioS;
    //[SerializeField] private GameObject RunEffect;
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
            StartCoroutine(RunEffectStart());
            audioS.Stop();
        }
    }

    IEnumerator RunEffectStart()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public override void AttackEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("AttackEventRPC", RpcTarget.All);
    }

    [PunRPC]
    public void AttackEventRPC()
    {
        Debug.Log("3");
        GameObject ObjectObj = GameObject.FindGameObjectWithTag("Object");
        Object ObjectS = ObjectObj.GetComponent<Object>();
        SoundManager.Instance.PlayMonsterSfx(0, transform.position); // Edit
        ObjectS.Damaged(damage);
        attackTimer = attackCooldown;
        currentState = States.Idle;
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
        currentState = States.Stun;
        animator.SetTrigger("Stun");        
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
        currentState = States.Idle;
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
    }
}
