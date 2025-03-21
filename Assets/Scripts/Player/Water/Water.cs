using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Water : PlayerController
{
    public PlayerController[] AllPlayers;

    public GameObject repellingWave;
    public GameObject repellingWavePlayerEF;

    public GameObject callOfTheSea;

    private Vector3[] skillsPos = new Vector3[3];

    public override void StartStatSet()
    {
        base.StartStatSet();
        AllPlayers = FindObjectsOfType<PlayerController>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().pv.RPC("OnPlayerRecovery", RpcTarget.All, (playerMaxHp * 0.02f));
            }
        }
    }

    public override void Dash() // 대쉬 애니메이션, 이펙트 추가하기
    {
        base.Dash();
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
                        Vector3 direction = GetMousePosition() - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;

                        skillRanges[0].transform.position = transform.position + direction * 10;
                        skillRanges[0].transform.rotation = Quaternion.LookRotation(GetSkillRange(10) - transform.position);
                    }
                    if (Input.GetKeyUp(KeyCode.Q))
                    {
                        Vector3 direction = GetMousePosition() - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;
                        skillsPos[0] = transform.position + direction * 20;
                        transform.rotation = Quaternion.LookRotation(GetSkillRange(10) - transform.position);
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

    public void StopAnimation() // 애니메이션 종료시 실행시킬 함수
    {
        currentStates = States.Idle;
    }

    public void UseRepellingWave()
    {
        if (pv.IsMine)
            pv.RPC("RepellingWave", RpcTarget.All, skillsPos[0]);
    }

    public void UseHealingBubble() // 애니메이션 이벤트로 실행시킬 함수
    {
        if (pv.IsMine)
            pv.RPC("HealingBubble", RpcTarget.All, null);
    }

    public void UseCalOfTheSea()
    {
        if (pv.IsMine)
            pv.RPC("CallOfTheSea", RpcTarget.All, skillsPos[2]);
    }

    [PunRPC]
    public void RepellingWave(Vector3 _targetPos)
    {
        repellingWavePlayerEF.SetActive(false);
        repellingWavePlayerEF.SetActive(true);
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 180));
        GameObject skill = Instantiate(repellingWave, transform.position, fireRot);
        skill.GetComponent<RepellingWave>().targetPos = _targetPos;
        skill.GetComponent<RepellingWave>().damage = playerAtk;
    }

    [PunRPC]
    public void HealingBubble()
    {
        foreach (PlayerController player in AllPlayers)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 7.5)
            {
                player.pv.RPC("RecoveryShield", RpcTarget.All, playerMaxHp * 0.1f);
            }
        }
    }

    [PunRPC]
    public void CallOfTheSea(Vector3 _targetPos)
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        GameObject skill = Instantiate(callOfTheSea, _targetPos, fireRot);
        skill.GetComponent<CallOfTheSea>().damage = playerAtk;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
        if (stream.IsWriting)
        {
            stream.SendNext(skillsPos[0]);
            stream.SendNext(skillsPos[1]);
        }
        else
        {
            skillsPos[0] = (Vector3)stream.ReceiveNext();
            skillsPos[1] = (Vector3)stream.ReceiveNext();
        }
    }
}