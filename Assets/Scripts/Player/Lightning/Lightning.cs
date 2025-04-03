using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Lightning : PlayerController
{
    public GameObject speedUpEF;

    public GameObject shockSpin;

    float[] playerSpeedUp;

    public override void StartStatSet()
    {
        base.StartStatSet();
        playerSpeedUp = new float[] { playerSpeed, playerSpeed + 1 };
    }
    public override void Update()
    {
        base.Update();
        if (speedUpCoroutine != null)
            playerSpeed = playerSpeedUp[1];
        else
            playerSpeed = playerSpeedUp[0];
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Enemy")
            {
                pv.RPC("OnPlayerSpeedUp", RpcTarget.All, null);
            }
        }
    }

    public Coroutine speedUpCoroutine;
    [PunRPC]
    void OnPlayerSpeedUp()
    {
        if (speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
        }

        speedUpCoroutine = StartCoroutine("PlayerSpeedUp");
    }

    IEnumerator PlayerSpeedUp()
    {
        speedUpEF.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        speedUpEF.SetActive(false);
        speedUpCoroutine = null;
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
                        skillRanges[0].SetActive(true);
                    }
                    if (Input.GetKeyUp(KeyCode.Q))
                    {
                        skillRanges[0].SetActive(false);
                        currentSkillsCoolTime[0] = skillsCoolTime[0];
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
                        Debug.Log("123123");
                    }
                }
            }
            ElementalSetting();
        }
    }

    public void StopAnimation() // 애니메이션 종료시 실행시킬 함수
    {
        currentStates = States.Idle;
    }

    public void UseShockSpin()
    {
        if (pv.IsMine)
            pv.RPC("ShockSpin", RpcTarget.All, null);
    }

    [PunRPC]
    public void ShockSpin()
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(90, 0, -20));
        GameObject skill = Instantiate(shockSpin, transform.position, fireRot);
        skill.GetComponent<ShockSpin>().damage = playerAtk;
    }
}