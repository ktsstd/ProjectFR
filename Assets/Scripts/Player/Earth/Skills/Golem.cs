using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    PhotonView pv;
    
    public GameObject summonEF;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
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
