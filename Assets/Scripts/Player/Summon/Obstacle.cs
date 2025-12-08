using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Obstacle : MonoBehaviourPunCallbacks
{
    public float currentHp;
    public float maxHp;
    public float summonTime;
    bool isDistroy;

    Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("DistroyAni", summonTime);
    }

    public void StartSetting(float _hp, float _time)
    {
        currentHp = _hp;
        maxHp = _hp;
        summonTime = _time;
    }

    [PunRPC]
    public void OnObstacleHit(float _damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isDistroy)
            {
                float currentDamage = _damage;

                currentHp -= currentDamage;

                if (currentHp <= 0)
                {
                    currentHp = 0;
                    isDistroy = true;
                    animator.SetTrigger("Distroy");
                }
            }
        }
    }

    public void DistroyAni()
    {
        animator.SetTrigger("Distroy");
    }

    public void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}