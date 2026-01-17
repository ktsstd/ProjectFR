using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Fence : MonsterAI
{
    public float currentHp;
    public float maxHp;

    public override void Awake()
    {

    }
    public void Start()
    {
        currentHp = maxHp;
    }
    public override void Update()
    {
        
    }
    public override void MonsterDmged(float damage, int playercode)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("OnMonsterHit", RpcTarget.All, damage);
    }

    [PunRPC]
    public override void OnMonsterHit(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        FenceHpUpdate();
    }
    public void FenceHpUpdate()
    {
        if (HpBarObj.activeSelf == false)
        {
            HpBarObj.SetActive(true);
        }
        else
        {
            BarInv HpBarS = HpBarObj.GetComponent<BarInv>();
            HpBarS.ResetTimer();
        }
        HpBar.value = currentHp / maxHp;
    }
}
