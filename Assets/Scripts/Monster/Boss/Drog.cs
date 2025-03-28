using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.ComponentModel;
using UnityEngine.UIElements;
using System.Security.Cryptography;

public class Drog : MonsterAI
{
    private float FirPatternbreakupHealth = 1500f;
    private float FirPatternHealth;
    private int BossPhase;
    public float[] BossMonsterSkillCooldowns = { 3f, 10f, 10f, 10f };
    public float[] BossMonsterSkillTimers = new float[4];

    [SerializeField] Transform MouthPos;
    
    [SerializeField] GameObject FSkill3Obj;
    [SerializeField] GameObject BossSkill1Obj;
    [SerializeField] GameObject BossJumpSkill;
    [SerializeField] GameObject BossLandSkill;
    [SerializeField] GameObject BossSkill3Obj;
    [SerializeField] GameObject BossSkill4Obj;

    [SerializeField] BossSkill1Script boss1Script;
    [SerializeField] BossSkill2Script boss2Script;
    [SerializeField] BossSkill3Script boss3Script;
    [SerializeField] BossSkill4Script boss4Script;
    


    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        BossPhase = 1;
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Keypad8))
            MonsterDmged(1000f);
        for (int i = 0; i < BossMonsterSkillTimers.Length; i++)
        {
            if (BossMonsterSkillTimers[i] > 0f)
            {
                BossMonsterSkillTimers[i] -= Time.deltaTime;
            }
        }
        if (FirPatternHealth > 0 && FSkill3Obj != null)
        {
            FSkill3Obj.transform.position = gameObject.transform.position;
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
        canMove = false;
        switch (randomskill)
        {
            case 0:
                BossSkill1Obj.SetActive(true);
                boss1Script.Starting();
                yield return new WaitForSeconds(0.641f);
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
                GameObject AttackObj2 = Instantiate(BossLandSkill, attackFowardPos2, transform.rotation);
                transform.position = attackFowardPos2;
                AttackObj2.transform.SetParent(this.transform);
                Vector3 AttackObj2local = AttackObj2.transform.localPosition;
                AttackObj2local.y = -0.85f;
                AttackObj2.transform.localPosition = AttackObj2local;
                break;
            case 2:
                BossSkill3Obj.SetActive(true);
                boss3Script.Starting();
                yield return new WaitForSeconds(0.6f);
                if (animator != null)
                    animator.SetTrigger("Skill3");
                break;
            case 3:
                if (animator != null)
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
    public void Skill3Success(GameObject Obj)
    {
        FSkill3Obj = Obj;
        //photonView.RPC("Skill3Start", RpcTarget.All);
        Skill3Start();
        if (animator != null)
            animator.SetTrigger("Skill3_1");
    }
    //[PunRPC]
    public void Skill3Start()
    {
        FirPatternHealth = FirPatternbreakupHealth;
        PlayerController playerScript = FSkill3Obj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("OnPlayerSuppressed", RpcTarget.All, 15f);
        FSkill3Obj.transform.position = gameObject.transform.position;
        is3Patterning = true;
        skill3Coroutine = StartCoroutine(Skill3PatternStart(FSkill3Obj));
    }
    bool is3Patterning;
    IEnumerator Skill3PatternStart(GameObject Obj)
    { 
        yield return new WaitForSeconds(15f);
        if (!is3Patterning) yield break;
        PlayerController playerScript = Obj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("OnPlayerHit", RpcTarget.All, 20000f);
        playerScript.photonView.RPC("PlayerStunClear", RpcTarget.All);
        FSkill3Obj = null;
        FirPatternHealth = 0;
        Obj.transform.position = MouthPos.transform.position;
        if (animator != null)
            animator.SetTrigger("Skill3__1");
        yield return new WaitForSeconds(2f);
        target = null;
        canMove = true;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
    }
    //[PunRPC]
    public IEnumerator Spititout()
    {
        //string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[5].name;
        //Vector3 attackFowardPos5 = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1;
        //GameObject SpitObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos5, Quaternion.identity);
        is3Patterning = false;
        PlayerController playerScript = FSkill3Obj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("PlayerStunClear", RpcTarget.All);
        FSkill3Obj.transform.position = MouthPos.transform.position;
        FSkill3Obj = null;
        if (animator != null)
            animator.SetTrigger("Skill3_2");
        yield return new WaitForSeconds(2f);
        canMove = true;
        BossMonsterSkillTimers[2] = BossMonsterSkillCooldowns[2];
    }
    public override void MonsterDmged(float damage)
    {
        if (FirPatternHealth <= 0)
        {
            CurHp -= damage;
            if (CurHp <= 0 && canMove)
            {
                canMove = false;
                if (BossPhase < 2)
                {
                    photonView.RPC("PunAttack", RpcTarget.All, 2);
                    CurHp = 13000f;
                    monsterInfo.damage = 100f;
                    BossPhase = 2;
                }
                else
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("Die");
                    }
                }
            }
        }
        else
        {
            FirPatternHealth -= damage;
            if (FirPatternHealth <= 0)
            {
                StopCoroutine(skill3Coroutine);
                StartCoroutine(Spititout());
                //photonView.RPC("Spititout", RpcTarget.All);
            }
        }
    }
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(CurHp);
            stream.SendNext(FirPatternHealth);
            stream.SendNext(canMove);
            stream.SendNext(BossPhase);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            CurHp = (float)stream.ReceiveNext();
            FirPatternHealth = (float)stream.ReceiveNext();
            canMove = (bool)stream.ReceiveNext();
            BossPhase = (int)stream.ReceiveNext();
        }
    }
}
