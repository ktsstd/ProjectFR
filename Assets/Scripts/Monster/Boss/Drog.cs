using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Drog : MonsterAI
{
    private float FirPatternbreakupHealth = 1500f;
    private float FirPatternHealth;
    private int BossPhase;
    public float[] BossMonsterSkillCooldowns = { 3f, 10f, 10f, 10f };
    public float[] BossMonsterSkillTimers = new float[4];

    public GameObject BossSkill3;
    public GameObject BossJumpSkill;
    //private GameObject FSkill3Obj;
    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        BossPhase = 1;
    }
    public override void Update()
    {
        base.Update();
        for (int i = 0; i < BossMonsterSkillTimers.Length; i++)
        {
            if (BossMonsterSkillTimers[i] > 0f)
            {
                BossMonsterSkillTimers[i] -= Time.deltaTime;
            }
        }
    }
    public override void Attack()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomskill = GetRandomSkill();
            photonView.RPC("PunAttack", RpcTarget.All, randomskill);
        }
        else
        {
            Debug.Log("Not Master Client");
        }
    }
    private int GetRandomSkill()
    {
        List<int> availableSkills = new List<int>();

        for (int i = 0; i < BossMonsterSkillTimers.Length; i++)
        {
            if (BossMonsterSkillTimers[i] <= 0f)
            {
                if (i == 2 || i == 3)
                {
                    if (BossPhase >= 2)
                    {
                        availableSkills.Add(i);
                    }
                }
                else
                {
                    availableSkills.Add(i);
                }

            }
        }

        if (availableSkills.Count > 0)
        {
            return availableSkills[Random.Range(0, availableSkills.Count)];
        }
        else
        {
            monsterInfo.attackTimer = monsterInfo.attackCooldown;
        }
        return -1;
    }
    [PunRPC]
    public IEnumerator PunAttack(int randomskill)
    {
        switch (randomskill)
        {
            case 0:
                //Vector3 attackFowardPos1 = new Vector3(transform.position.x, 0.02f, transform.position.z) + transform.forward * 9;
                //GameObject AttackObj1 = Instantiate(monsterInfo.attackboundary[0], attackFowardPos1, transform.rotation);
                //AttackObj1.transform.SetParent(this.transform);
                //Vector3 AttackObj1local = AttackObj1.transform.localPosition;
                //AttackObj1local.y = -0.85f;
                //AttackObj1.transform.localPosition = AttackObj1local;
                yield return new WaitForSeconds(0.6f);
                if (animator != null)
                    animator.SetTrigger("Skill1");
                break;
            case 1:
                if (animator != null)
                    animator.SetTrigger("Skill2");
                Collider collider = GetComponent<Collider>();
                collider.enabled = false;
                Instantiate(BossJumpSkill, transform.position, Quaternion.identity);
                Vector3 attackFowardPos2 = new Vector3(target.position.x, 0.02f, target.position.z);
                yield return new WaitForSeconds(0.6f);
                GameObject AttackObj2 = Instantiate(monsterInfo.attackboundary[1], attackFowardPos2, transform.rotation);
                transform.position = attackFowardPos2;
                AttackObj2.transform.SetParent(this.transform);
                Vector3 AttackObj2local = AttackObj2.transform.localPosition;
                AttackObj2local.y = -0.85f;
                AttackObj2.transform.localPosition = AttackObj2local;
                break;
            case 2:
                //Vector3 attackFowardPos3 = new Vector3(transform.position.x, 0.02f, transform.position.z) + transform.forward * 0;
                //Vector3 currentEulerAngles = transform.eulerAngles;
                //GameObject AttackObj3 = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos3, Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
                //AttackObj3.transform.SetParent(this.transform);
                //Vector3 AttackObj3local = AttackObj3.transform.localPosition;
                //AttackObj3local.y = -0.9f;
                //AttackObj3.transform.localPosition = AttackObj3local;
                BossSkill3.SetActive(true);
                BossSkill3Script boss3Script = BossSkill3.GetComponent<BossSkill3Script>();
                boss3Script.Starting();
                yield return new WaitForSeconds(0.6f);
                if (animator != null)
                    animator.SetTrigger("Skill3");
                break;
            case 3:
                if (animator != null)
                    animator.SetTrigger("Skill4");
                Vector3 attackFowardPos4 = new Vector3(transform.position.x, 0.02f, transform.position.z);
                GameObject AttackObj4 = Instantiate(monsterInfo.attackboundary[3], attackFowardPos4, transform.rotation);
                AttackObj4.transform.SetParent(this.transform);
                Vector3 AttackObj4local = AttackObj4.transform.localPosition;
                AttackObj4local.y = -0.85f;
                AttackObj4.transform.localPosition = AttackObj4local;
                break;
            default:
                canMove = true;
                yield break;
        }
    }
}
