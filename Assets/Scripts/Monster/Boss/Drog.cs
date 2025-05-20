using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Drog : MonsterAI
{
    float PatternbreakupHealth;
    float PatternHealth;
    public float BossPhase2Hp;
    public float[] BossMonsterSkillCooldowns = { 3f, 10f, 10f, 10f };
    public float[] BossMonsterSkillTimers = new float[4];

    public int BossPhase = 1;

    [SerializeField] GameObject BossSkill1Obj;
    [SerializeField] GameObject BossJumpSkill;
    [SerializeField] GameObject BossLandSkill;
    [SerializeField] GameObject BossSkill3Obj;
    [SerializeField] GameObject BossSkill3_2Obj;
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
        PatternbreakupHealth = 3000f;
        BossPhase2Hp = 39000f;
        animator = GetComponentInChildren<Animator>();
        if (PhotonNetwork.PlayerList.Length <= 1)
        {
            BossPhase2Hp /= 2;
        }
        for (int i = 0; i < BossMonsterSkillCooldowns.Length; i++)
        {
            BossMonsterSkillCooldowns[i] = BossMonsterSkillCooldowns[i];
            BossMonsterSkillTimers[i] = BossMonsterSkillCooldowns[i];
        }
    }

    public override void Update()
    {
        if (sloweffect != null)
        {
            ParticleSystem ps = sloweffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            targetSearchTimer -= Time.deltaTime;
            if (targetSearchTimer <= 0f)
            {
                photonView.RPC("GetClosestTarget", RpcTarget.All);
                targetSearchTimer = targetSearchTime;
            }
        }
        else
        {

        }
        if (target == null) return;
        targetCollider = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = targetCollider.ClosestPoint(transform.position);
        if (canMove && CurHp > 0)
        {
            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance <= attackRange)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                animator.SetBool("Run", false);
                if (attackTimer <= 0)
                {
                    canMove = false;
                    Attack();
                }
                else
                {
                    Vector3 directionToTarget = target.position - transform.position;
                    if (directionToTarget != Vector3.zero || swallowedTarget == null)
                    {
                        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
                    }
                }
            }
            else
            {
                agent.SetDestination(target.position);
                animator.SetBool("Run", true);
            }
        }
        else
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("Run", false);
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
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
        if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                photonView.RPC("OnMonsterHit", RpcTarget.All, 1005f);
            }
    }
    [PunRPC]
    public void AddSwallowedTarget(string objname)
    {
        GameObject playerobj = GameObject.Find(objname);
        swallowedTarget.Add(playerobj);
        Debug.Log(objname);
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
                boss3Script.Starting(0.9f);
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
        foreach (GameObject playerObj in swallowedTarget)
        {
            Debug.Log(playerObj);
        }
        PatternHealth = PatternbreakupHealth;
        canMove = false;
        Skill3Start();
        animator.ResetTrigger("Skill3__1");
        animator.ResetTrigger("Skill3Over");
        animator.ResetTrigger("Skill3_2");
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
        if (swallowedTarget == null)
        {
            yield break;
        }
        foreach (GameObject playerObj in swallowedTarget)
        {
            PlayerController playerS = playerObj.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerHit", RpcTarget.AllBuffered, 20000f);
            playerS.photonView.RPC("PlayerStunClear", RpcTarget.AllBuffered);
        }
        photonView.RPC("ClearHash", RpcTarget.All);
        PatternHealth = 0;
        animator.SetTrigger("Skill3__1");
        yield return new WaitForSeconds(2f);
        target = null;
        canMove = true;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
        attackTimer = attackCooldown;
    }
    [PunRPC]
    public void Spititout()
    {
        StopCoroutine(skill3Coroutine);
        StartCoroutine(SpititoutC());
    }
    IEnumerator SpititoutC()
    {        
        foreach (GameObject playerObj in swallowedTarget)
        {
            PlayerController playerS = playerObj.GetComponent<PlayerController>();
            playerS.photonView.RPC("PlayerStunClear", RpcTarget.AllBuffered);
            playerS.transform.position = MouthPos.transform.position;
            playerObj.transform.position = new Vector3(playerObj.transform.position.x, 0, playerObj.transform.position.z);
        }
        photonView.RPC("ClearHash", RpcTarget.All);
        animator.SetTrigger("Skill3_2");
        ParticleSystem BossSkill32P = BossSkill3_2Obj.GetComponent<ParticleSystem>();
        BossSkill32P.Play();
        BossSkill3_2Obj.SetActive(true);
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
                    PlayerController playerS = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                    playerS.photonView.RPC("LookAtTarget", RpcTarget.All, gameObject.name, 55555f);
                    Invoke("GameEnd", 6f);
                }
            }
        }
        else
        {
            PatternHealth -= damage;
            if (PatternHealth <= 0)
            {
                photonView.RPC("Spititout", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void ClearHash()
    {
        swallowedTarget.Clear();
    }

    public virtual void GameEnd()
    {
        GameManager.Instance.GoToMain();
    }
}
