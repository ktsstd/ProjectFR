using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boss : MonsterAI
{
    private float FirPatternbreakupHealth = 1500f;
    private float FirPatternHealth;
    private int BossPhase;
    public float[] BossMonsterSkillCooldowns = { 3f, 10f, 10f, 10f };
    public float[] BossMonsterSkillTimers = new float[4];

    public GameObject BossSkill3;
    public GameObject BossJumpSkill;
    private GameObject FSkill3Obj;

    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        BossPhase = 1;
        // todo -> directional light color change
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            animator.SetTrigger("Skill1");
        if (Input.GetKeyDown(KeyCode.Keypad1))
            animator.SetTrigger("Skill2");
        if (Input.GetKeyDown(KeyCode.Keypad2))
            animator.SetTrigger("Skill3");
        if (Input.GetKeyDown(KeyCode.Keypad3))
            animator.SetTrigger("Skill3_1");
        if (Input.GetKeyDown(KeyCode.Keypad4))
            animator.SetTrigger("Skill3_2");
        if (Input.GetKeyDown(KeyCode.Keypad5))
            animator.SetTrigger("Skill4");
        if (Input.GetKeyDown(KeyCode.Keypad8))
            MonsterDmged(1000f);

        target = GetClosestTarget();        

        if (target != null && canMove && agent.enabled)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (animator != null)
                animator.SetBool("Run", true);

            if (distance <= monsterInfo.attackRange)
            {
                agent.velocity = Vector3.zero;
                if (monsterInfo.attackTimer <= 0)
                {
                    Attack();
                    canMove = false;
                }
                else
                {
                    agent.ResetPath(); // todo -> Idle animation
                }
                if (animator != null)
                    animator.SetBool("Run", false);
            }
            else
            {
                agent.SetDestination(target.position); // todo -> moving animation
                if (animator != null)
                    animator.SetBool("Run", true);
            }

            if (monsterInfo.attackTimer > 0)
            {
                monsterInfo.attackTimer -= Time.deltaTime;
            }
            Vector3 directionToTarget = target.position - transform.position;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                //if (animator != null && photonView.IsMine)
                //    animator.SetBool("Run", true);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
        else if (!canMove && agent.enabled)
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath(); // todo -> Idle animation
            if (animator != null)
                animator.SetBool("Run", false);
            target = GetClosestTarget();
        }
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
            //string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[randomskill].name;
            // BossMonsterSkillTimers[randomskill] = BossMonsterSkillCooldowns[randomskill];
            photonView.RPC("PunAttack", RpcTarget.All, randomskill);
        }     
    }

    [PunRPC]
    public void PunAttack(int randomskill)
    {
        StartCoroutine(PunAttackCoroutine(randomskill));
    }

    private IEnumerator PunAttackCoroutine(int randomskill)
    {
        switch (randomskill)
        {
            case 0:
                Vector3 attackFowardPos1 = new Vector3(transform.position.x, 0.02f, transform.position.z) + transform.forward * 9;
                GameObject AttackObj1 = Instantiate(monsterInfo.attackboundary[0], attackFowardPos1, transform.rotation);
                AttackObj1.transform.SetParent(this.transform);
                Vector3 AttackObj1local = AttackObj1.transform.localPosition;
                AttackObj1local.y = -0.85f;
                AttackObj1.transform.localPosition = AttackObj1local;
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


    public void Skill1Success(GameObject Obj, int damage)
    {
        Debug.Log("attack success: " + Obj + ":" + damage);
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        canMove = true;
    }

    Coroutine skill3Coroutine;
    public void Skill3Success(GameObject Obj)
    {
        FSkill3Obj = Obj;
        photonView.RPC("Skill3Start", RpcTarget.All);
    }
    [PunRPC]
    public void Skill3Start()
    {
        if (animator != null)
            animator.SetTrigger("Skill3_1");
        FirPatternHealth = FirPatternbreakupHealth;
        PlayerController playerScript = FSkill3Obj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("OnPlayerSuppressed", RpcTarget.All, 15f);
        FSkill3Obj.transform.position = gameObject.transform.position;
        skill3Coroutine = StartCoroutine(Skill3PatternStart(FSkill3Obj));
    }
    IEnumerator Skill3PatternStart(GameObject Obj)
    {
        yield return new WaitForSeconds(15f);
        PlayerController playerScript = Obj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("PlayerStunClear", RpcTarget.All, null);
        playerScript.photonView.RPC("OnPlayerHit", RpcTarget.All, 20000f);
        if (animator != null)
            animator.SetTrigger("Skill3__1");
        playerScript.photonView.RPC("PlayerStunClear", RpcTarget.All, null);
        yield return new WaitForSeconds(5f);
        target = null;
        canMove = true;
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
    }
    [PunRPC]
    IEnumerator Spititout()
    {
        //string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[5].name;
        //Vector3 attackFowardPos5 = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1;
        //GameObject SpitObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos5, Quaternion.identity);
        if (animator != null)
            animator.SetTrigger("Skill3_2");
        PlayerController playerScript = FSkill3Obj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("PlayerStunClear", RpcTarget.All);
        yield return new WaitForSeconds(2f);
        canMove = true;
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
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
    bool isDie = false;
    public override void MonsterDmged(float damage)
    {
        if (CurHp > 0 && FirPatternHealth <= 0) // Phase
        {
            CurHp -= damage;
            //Vector3 currentEulerAngles = transform.eulerAngles;
            //GameObject HitEffect = Instantiate(BossHit, transform.position, Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
            //HitEffect.transform.SetParent(this.transform);
        }
        else if (FirPatternHealth > 0)
        {
            FirPatternHealth -= damage;
            if (FirPatternHealth <= 0)
            {
                StopCoroutine(skill3Coroutine);
                photonView.RPC("Spititout", RpcTarget.All);
            }
        }
        else if (CurHp <= 0 && BossPhase < 2)
        {
            if (canMove)
            {
                canMove = false;
                photonView.RPC("PunAttack", RpcTarget.All, 2);
                CurHp = 13000f;
                monsterInfo.damage = 100f;
                BossPhase += 1;
            }
            else
            {
                return;
            }
        }
        else if (CurHp <= 0 && BossPhase >= 2)
        {
            canMove = false;
            if (isDie) return;
            isDie = true;
            canMove = false;
            if (animator != null)
                animator.SetTrigger("Die");
            canMove = false;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (stateInfo.IsName("Boss_Die") && animTime >= 1f)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
