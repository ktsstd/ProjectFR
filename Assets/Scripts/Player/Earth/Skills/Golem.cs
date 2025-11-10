using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Golem : SummonAI
{
    float earthShield = 0;

    public Transform attackPos;
    public GameObject attackEF;
    public GameObject summonEF;
    public GameObject earthShieldEF;

    Vector3 targetPos;

    public override void Start()
    {
        base.Start();
        currentHp = maxHp;
    }

    public override void AttackAnimation()
    {
        if (targetMonster != null)
            transform.rotation = Quaternion.LookRotation(targetMonster.transform.position - transform.position);
        SoundManager.Instance.PlayPlayerSfx(21, transform.position);
        GameObject skill = Instantiate(attackEF, attackPos.position, attackEF.transform.rotation);
        skill.GetComponent<Shockwave>().damage = atk;
    }

    [PunRPC]
    public IEnumerator Protection()
    {
        earthShieldEF.SetActive(true);
        earthShield = maxHp * 0.05f;
        yield return new WaitForSeconds(5f);
        earthShieldEF.SetActive(false);
        earthShield = 0;
    }

    public void UseSummonAnimationEffect()
    {
        if (pv.IsMine)
            pv.RPC("SummonAnimationEffect", RpcTarget.All, null);
    }

    [PunRPC]
    public void SummonAnimationEffect()
    {
        SoundManager.Instance.PlayPlayerSfx(20, transform.position);
        GameObject skill = Instantiate(summonEF, transform.position, summonEF.transform.rotation);
        skill.GetComponent<SumonDamage>().damage = atk;
    }
    public override float Shield(float _damage)
    {
        float damage = _damage;
        if (earthShield != 0)
        {
            if (earthShield < _damage)
            {
                damage = _damage - earthShield;
                earthShield = 0;
                pv.RPC("ProtectionBroken", RpcTarget.All, null);
            }
            else
            {
                earthShield -= _damage;
                damage = 0;
            }
        }
        return damage;
    }


    [PunRPC]
    public void ProtectionBroken()
    {
        earthShieldEF.SetActive(false);
    }
}