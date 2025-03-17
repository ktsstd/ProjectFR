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

    public override void Dash() // 대쉬 애니메이션, 이펙트 추가하기
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
                    // 애니메이션 실행
                    Invoke("UseRepellingWave", 1f); // 원래는 애니메이션 이벤트로 실행시키기
                }
            }
        }
    }

    public void StopAnimation() // 애니메이션 종료시 실행시킬 함수
    {
        currentStates = States.Idle;
    }

    public void UseRepellingWave() // 애니메이션 이벤트로 실행시킬 함수
    {
        pv.RPC("RepellingWave", RpcTarget.All, skillRanges[0].transform.position * 2);
        StopAnimation(); // 애니메이션 추가하면 빼기
    }

    [PunRPC]
    public void RepellingWave(Vector3 _targetPos)
    {
        GameObject skill = Instantiate(repellingWave, transform.position, repellingWave.transform.rotation);
        skill.GetComponent<RepellingWave>().targetPos = _targetPos;
    }
}