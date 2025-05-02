using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Drog : MonsterAI
{
    float PatternbreakupHealth;
    float PatternHealth;
    float Skill3Speed;
    public float BossPhase2Hp;
    public float[] BossMonsterSkillCooldowns = { 3f, 10f, 10f, 10f };
    public float[] BossMonsterSkillTimers = new float[4];

    public int BossPhase = 1;

    [SerializeField] GameObject BossSkill1Obj;
    [SerializeField] GameObject BossJumpSkill;
    [SerializeField] GameObject BossLandSkill;
    [SerializeField] GameObject BossSkill3Obj;
    [SerializeField] GameObject BossSkill4Obj;

    [SerializeField] BossSkill1Script boss1Script;
    [SerializeField] BossSkill2Script boss2Script;
    [SerializeField] BossSkill3Script boss3Script;
    [SerializeField] BossSkill4Script boss4Script;

    [SerializeField] Transform MouthPos;

    //public List<GameObject> Skill3Obj;
    public HashSet<GameObject> swallowedTarget = new HashSet<GameObject>();

    public override void Start()
    {
        base.Start();
        Skill3Speed = 0.9f;
        PatternbreakupHealth = 1500f;
        BossPhase2Hp = 13000f;
        PatternHealth = PatternbreakupHealth;
        animator = GetComponentInChildren<Animator>();
        if (PhotonNetwork.PlayerList.Length <= 1)
        {
            BossPhase2Hp /= 2;
        }
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
        if (PatternHealth > 0)
        {
            foreach (GameObject obj in swallowedTarget)
            {
                if (obj != null)
                {
                    obj.transform.position = gameObject.transform.position;
                }
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
            attackTimer = attackCooldown;
            canMove = true;
        }
        return -1;
    }
    [PunRPC]
    public IEnumerator PunAttack(int randomskill)
    {
        canMove = false;
        switch (randomskill)
        {
            case 0:
                BossSkill1Obj.SetActive(true);
                boss1Script.Starting();
                yield return new WaitForSeconds(0.641f);
                animator.SetTrigger("Skill1");
                break;
            case 1:
                animator.SetTrigger("Skill2");
                Collider collider = GetComponent<Collider>();
                collider.enabled = false;
                Instantiate(BossJumpSkill, transform.position, Quaternion.identity);
                Vector3 attackFowardPos2 = new Vector3(target.position.x, 0.02f, target.position.z);
                yield return new WaitForSeconds(0.6f);
                GameObject AttackObj2 = Instantiate(BossLandSkill, attackFowardPos2, transform.rotation);
                transform.position = attackFowardPos2;
                AttackObj2.transform.SetParent(this.transform);
                Vector3 AttackObj2local = AttackObj2.transform.localPosition;
                AttackObj2local.y = -0.85f;
                AttackObj2.transform.localPosition = AttackObj2local;
                break;
            case 2:
                BossSkill3Obj.SetActive(true);
                boss3Script.Starting(Skill3Speed);
                yield return new WaitForSeconds(0.6f);
                animator.SetTrigger("Skill3");
                break;
            case 3:
                animator.SetTrigger("Skill4");
                BossSkill4Obj.SetActive(true);
                boss4Script.Starting();
                break;
            default:
                canMove = true;
                yield break;
        }
    }
    Coroutine skill3Coroutine;
    [PunRPC]
    public void Skill3Success()
    {
        PatternHealth = PatternbreakupHealth;
        canMove = false;
        Skill3Start();
        animator.SetTrigger("Skill3_1");
    }
    public void Skill3Start()
    {
        foreach (GameObject playerObj in swallowedTarget)
        {
            PlayerController playerS = playerObj.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerSuppressed", RpcTarget.All, 15f);
        }
        skill3Coroutine = StartCoroutine(Skill3PatternStart());
    }
    IEnumerator Skill3PatternStart()
    {
        yield return new WaitForSeconds(15f);
        foreach (GameObject playerObj in swallowedTarget)
        {
            PlayerController playerS = playerObj.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerHit", RpcTarget.AllBuffered, 20000f);
            playerS.photonView.RPC("PlayerStunClear", RpcTarget.AllBuffered);
        }
        swallowedTarget.Clear();
        PatternHealth = 0;
        animator.SetTrigger("Skill3__1");
        yield return new WaitForSeconds(2f);
        target = null;
        canMove = true;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
        attackTimer = attackCooldown;
    }
    IEnumerator Spititout()
    {
        StopCoroutine(skill3Coroutine);
        foreach (GameObject playerObj in swallowedTarget)
        {
            PlayerController playerS = playerObj.GetComponent<PlayerController>();
            playerS.photonView.RPC("PlayerStunClear", RpcTarget.AllBuffered);
            playerS.transform.position = MouthPos.transform.position;
        }
        swallowedTarget.Clear();
        animator.SetTrigger("Skill3_2");
        yield return new WaitForSeconds(2f);
        canMove = true;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
        attackTimer = attackCooldown;
    }
    [PunRPC]
    public override void OnMonsterHit(float damage)
    {
        if (PatternHealth <= 0)
        {
            CurHp -= damage;
            if (CurHp <= 0 && canMove)
            {
                canMove = false;
                if (BossPhase < 2)
                {
                    photonView.RPC("PunAttack", RpcTarget.All, 2);
                    CurHp = BossPhase2Hp;
                    damage = 100f;
                    BossPhase = 2;
                }
                else
                {
                    animator.SetTrigger("Die");
                }
            }
        }
        else
        {
            PatternHealth -= damage;
            if (PatternHealth <= 0)
            {
                StartCoroutine(Spititout());
            }
        }
    }
}
