using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : SummonAI
{
    PhotonView pv;
    Animator animator;
    
    public GameObject summonEF;

    Vector3 targetPos;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    public void UseSummonAnimationEffect()
    {
        if (pv.IsMine)
            pv.RPC("SummonAnimationEffect", RpcTarget.All, null);
    }

    [PunRPC]
    public void SummonAnimationEffect()
    {
        Instantiate(summonEF, transform.position, summonEF.transform.rotation);
    }
}
