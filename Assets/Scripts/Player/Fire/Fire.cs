using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : PlayerController
{
    bool isFlameSpray = false;

    public ParticleSystem fireParticle;
    public GameObject flameSpray;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Enemy")
            {
                Debug.Log(5 + playerAtk * 0.1 + " 대쉬공격 데미지 부여");
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
                    }
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        skillRanges[1].SetActive(false);
                        currentSkillsCoolTime[1] = skillsCoolTime[1];
                        currentStates = States.Attack;
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
                        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill3");
                    }
                }
            }
        }
    }

    public override void Dash() // 이펙트 추가하기
    {
        base.Dash();
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

    [PunRPC]
    public void FlameSprayTest()
    {
        isFlameSpray = !isFlameSpray;
        if (isFlameSpray)
        {
            fireParticle.Play();
            flameSpray.SetActive(true);
            playerSpeed -= 1;
        }
        else
        {
            fireParticle.Stop();
            flameSpray.SetActive(false);
            playerSpeed += 1;
        }
    }

    public override void RunAnimation()
    {
        if (!isFlameSpray)
            animator.SetBool("isRun", isMoving);
        else
            animator.SetBool("isRun_2", isMoving);

    }
}
