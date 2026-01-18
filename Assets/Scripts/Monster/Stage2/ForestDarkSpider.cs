using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestDarkSpider : MonsterAI
{
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Attackboundary atkboundary;
    [SerializeField] GameObject DieObj;
    [SerializeField] ParticleSystem DieEffect;
    public void Start()
    {
        skinnedMeshRenderer.material.color = new Color(1f, 1f, 1f, 0.35f);
        currentState = States.Idle;
    }
    
    public override void ShowAttackBoundary()
    {
        if (currentState != States.Attack) return;
        atkboundary.ShowBoundary();
    }
    public override void AttackEvent()
    {
        if (currentState != States.Attack) return;
        atkboundary.EnterPlayer();
    }
    public override void MonsterDmged(float damage, int playercode)
    {
        skinnedMeshRenderer.material.color = new Color(1f, 1f, 1f, 1f);
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("SetLatestAttacker", RpcTarget.AllBuffered, playercode);
        photonView.RPC("OnMonsterHit", RpcTarget.All, damage);
    }
    [PunRPC]
    public override void OnMonsterHit(float damage)
    {
        if (currentState == States.Die) return;

        CurHp -= damage;

        if (CurHp <= 0)
        {
            currentState = States.Die;

            if (agent != null)
            {
                agent.ResetPath();
                agent.enabled = false;
            }

            animator.Rebind();
            animator.SetTrigger("Die");
            Invoke("DestroyMonster", 1.5f);
            DieObj.SetActive(true);
            DieEffect.Play();
        }

        HpUpdate();
    }
}
