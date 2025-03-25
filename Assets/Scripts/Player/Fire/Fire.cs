using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : PlayerController
{
    bool isFlameSpray = false;

    public ParticleSystem fireParticle;
    public GameObject flameSpray;

    public GameObject grenade;
    private int grenadeType;

    public GameObject flameGrenadeTest;

    public GameObject finalTest;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<MonsterAI>().MonsterDmged(5f + playerAtk * 0.1f);
            }
        }
    }

    public override void Attack()
    {
        if (pv.IsMine && currentStates == States.Idle)
        {
            if (!skillRanges[1].activeSelf && !skillRanges[2].activeSelf)
            {
                if (currentSkillsCoolTime[0] <= 0)
                {
                    if (Input.GetKey(KeyCode.Q))
                    {
                        if (!isFlameSpray)
                        {
                            skillRanges[0].SetActive(true);
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.Q))
                    {
                        currentSkillsCoolTime[0] = skillsCoolTime[0];
                        skillRanges[0].SetActive(false);
                        currentStates = States.Attack;
                        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill1");
                    }
                }
            }
            if (!skillRanges[0].activeSelf && !skillRanges[2].activeSelf)
            {
                if (currentSkillsCoolTime[1] <= 0)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        skillRanges[1].SetActive(true);
                        skillRanges[1].transform.position = new Vector3(GetSkillRange(10).x, 0.1f, GetSkillRange(10).z);
                    }
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        skillRanges[1].SetActive(false);
                        skillsPos[1] = new Vector3(GetSkillRange(10).x, 0.1f, GetSkillRange(10).z);
                        transform.rotation = Quaternion.LookRotation(GetSkillRange(10) - transform.position);
                        currentSkillsCoolTime[1] = skillsCoolTime[1];
                        currentStates = States.Attack;
                        grenadeType = 0;
                        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill2");
                    }
                }
            }
            if (!skillRanges[0].activeSelf && !skillRanges[1].activeSelf)
            {
                if (currentSkillsCoolTime[2] <= 0)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        skillRanges[2].SetActive(true);
                        skillRanges[2].transform.position = new Vector3(GetSkillRange(10).x, 0.1f, GetSkillRange(10).z);
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        skillRanges[2].SetActive(false);
                        skillsPos[2] = new Vector3(GetSkillRange(10).x, 0.1f, GetSkillRange(10).z);
                        transform.rotation = Quaternion.LookRotation(GetSkillRange(10) - transform.position);
                        currentSkillsCoolTime[2] = skillsCoolTime[2];
                        currentStates = States.Attack;
                        grenadeType = 1;
                        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill2");
                    }
                }
            }
        }
    }

    public void StopAnimation() // 애니메이션 종료시 실행시킬 함수
    {
        currentStates = States.Idle;
    }

    public void UseFireButton()
    {
        if (pv.IsMine)
            pv.RPC("FlameSprayTest", RpcTarget.All, null);
    }

    public void UseGrenade()
    {
        if (pv.IsMine)
        {
            if (grenadeType == 0)
                pv.RPC("Grenade", RpcTarget.All, skillsPos[1], grenadeType);
            else
                pv.RPC("Grenade", RpcTarget.All, skillsPos[2], grenadeType);
        }
            
    }

    [PunRPC]
    public void FlameSprayTest()
    {
        isFlameSpray = !isFlameSpray;
        if (isFlameSpray)
        {
            fireParticle.Play();
            flameSpray.SetActive(true);
            flameSpray.GetComponent<FlameSprayTest>().damage = playerAtk;
            playerSpeed -= 1;
        }
        else
        {
            fireParticle.Stop();
            flameSpray.SetActive(false);
            playerSpeed += 1;
        }
    }

    [PunRPC]
    public void Grenade(Vector3 _targetPos, int _grenadeType)
    {
        GameObject skill = Instantiate(grenade, transform.position, transform.rotation);
        Grenade grenadeScript = skill.GetComponent<Grenade>();
        grenadeScript.target = _targetPos;
        grenadeScript.skilltype = _grenadeType;
        grenadeScript.player = this;
    }

    [PunRPC]
    public void FlameGrenadeTest(Vector3 _targetPos)
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        GameObject skill = Instantiate(flameGrenadeTest, _targetPos, fireRot);
        skill.GetComponent<FlameGrenadeTest>().damage = playerAtk;
    }

    [PunRPC]
    public void FinalTest(Vector3 _targetPos)
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        GameObject skill = Instantiate(finalTest, _targetPos, fireRot);
        skill.GetComponent<FinalTest>().damage = playerAtk;
    }

    public override void RunAnimation()
    {
        if (!isFlameSpray)
            animator.SetBool("isRun", isMoving);
        else
            animator.SetBool("isRun_2", isMoving);
    }

    public override void PlayerDieEvent()
    {
        if (isFlameSpray)
        {
            fireParticle.Stop();
            flameSpray.SetActive(false);
            playerSpeed += 1;
        }
    }

    public override void OffSkills()
    {
        if (isFlameSpray)
        {
            fireParticle.Stop();
            flameSpray.SetActive(false);
            playerSpeed += 1;
        }
    }
}
