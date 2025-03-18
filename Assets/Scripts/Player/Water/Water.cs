using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Water : PlayerController
{
    public PlayerController[] AllPlayers;
    public GameObject repellingWave;

    private Vector3[] skillsPos = new Vector3[2];

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

    public override void Dash() // �뽬 �ִϸ��̼�, ����Ʈ �߰��ϱ�
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
                        // �ִϸ��̼� ����
                        Invoke("UseRepellingWave", 1f); // ������ �ִϸ��̼� �̺�Ʈ�� �����Ű��
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
                        // �ִϸ��̼� ����
                        Invoke("UseHealingBubble", 1f);
                    }
                }
            }
        }
    }

    public void StopAnimation() // �ִϸ��̼� ����� �����ų �Լ�
    {
        currentStates = States.Idle;
    }

    public void UseRepellingWave() // �ִϸ��̼� �̺�Ʈ�� �����ų �Լ�
    {
        pv.RPC("RepellingWave", RpcTarget.All, skillsPos[0]);
        StopAnimation(); // �ִϸ��̼� �߰��ϸ� ����
    }

    public void UseHealingBubble() // �ִϸ��̼� �̺�Ʈ�� �����ų �Լ�
    {

        pv.RPC("HealingBubble", RpcTarget.All, null);
        StopAnimation(); // �ִϸ��̼� �߰��ϸ� ����
    }

    [PunRPC]
    public void RepellingWave(Vector3 _targetPos)
    {
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