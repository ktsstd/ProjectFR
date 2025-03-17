using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Water : PlayerController
{
    public GameObject repellingWave;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().pv.RPC("OnPlayerHeal", RpcTarget.All, (playerMaxHp * 0.02f));
            }
        }
    }

    public override void Dash() // 대쉬 애니메이션, 이펙트 추가하기
    {
        base.Dash();
    }

    public override void Attack()
    {
        if (!Input.GetKey(KeyCode.E))
        {
            
        }
    }

    void UseRepellingWave()
    {

    }

}