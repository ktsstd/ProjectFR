using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Water : PlayerController
{
    public GameObject repellingWave;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().pv.RPC("OnPlayerHeal", RpcTarget.All, (playerMaxHp * 0.02f));
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
            if (!Input.GetKey(KeyCode.E) && currentSkillsCoolTime[0] <= 0)
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
                    transform.rotation = Quaternion.LookRotation(GetSkillRange(10) - transform.position);
                    currentSkillsCoolTime[0] = skillsCoolTime[0];
                    skillRanges[0].SetActive(false);
                    currentStates = States.Attack;
                    // �ִϸ��̼� ����
                    Invoke("UseRepellingWave", 1f); // ������ �ִϸ��̼� �̺�Ʈ�� �����Ű��
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
        pv.RPC("RepellingWave", RpcTarget.All, skillRanges[0].transform.position * 2);
        StopAnimation(); // �ִϸ��̼� �߰��ϸ� ����
    }

    [PunRPC]
    public void RepellingWave(Vector3 _targetPos)
    {
        GameObject skill = Instantiate(repellingWave, transform.position, repellingWave.transform.rotation);
        skill.GetComponent<RepellingWave>().targetPos = _targetPos;
    }
}