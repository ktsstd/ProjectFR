using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : PlayerController
{
    public GameObject shockwave;
    public GameObject earthShieldEF;

    public GameObject crystal;

    float earthShield = 0;

    public override void StartStatSet()
    {
        base.StartStatSet();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (currentStates == States.Dash)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<MonsterAI>().OnMonsterSpeedDown(3f, 1f);
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
                        skillRanges[0].SetActive(true);
                        Vector3 direction = GetMousePosition() - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;

                        skillRanges[0].transform.position = transform.position + direction * 1.5f;
                        skillRanges[0].transform.rotation = Quaternion.LookRotation(GetSkillRange(1.5f) - transform.position);
                    }
                    if (Input.GetKeyUp(KeyCode.Q))
                    {
                        Vector3 direction = GetMousePosition() - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;
                        skillsPos[0] = transform.position + direction * 1.5f;
                        transform.rotation = Quaternion.LookRotation(GetSkillRange(1.5f) - transform.position);
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
                    if (Input.GetKeyDown(KeyCode.W))
                    {
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
                        skillRanges[2].transform.position = new Vector3(GetSkillRange(5).x, 0.1f, GetSkillRange(5).z);
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        skillRanges[2].SetActive(false);
                        skillsPos[2] = new Vector3(GetSkillRange(5).x, 0.1f, GetSkillRange(5).z);
                        transform.rotation = Quaternion.LookRotation(GetSkillRange(5) - transform.position);
                        currentSkillsCoolTime[2] = skillsCoolTime[2];
                        currentStates = States.Attack;
                        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill3");
                    }
                }
            }
            ElementalSetting();
            UseItem();
        }
    }

    public void StopAnimation() // 애니메이션 종료시 실행시킬 함수
    {
        currentStates = States.Idle;
    }

    public void UseShockwave()
    {
        if (pv.IsMine)
            pv.RPC("Shockwave", RpcTarget.All, skillsPos[0]);
    }

    public void UseProtection()
    {
        if (pv.IsMine)
        {
            pv.RPC("Protection", RpcTarget.All, null);
            if (golem != null)
                golem.GetComponent<Golem>().pv.RPC("Protection", RpcTarget.All, null);
        }
    }

    public void UseGuardian()
    {
        if (pv.IsMine)
            pv.RPC("Guardian", RpcTarget.All, skillsPos[2]);
    }

    [PunRPC]
    public void Shockwave(Vector3 _targetPos)
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        GameObject skill = Instantiate(shockwave, _targetPos, fireRot);
        skill.GetComponent<Shockwave>().damage = playerAtk;
        SoundManager.Instance.PlayPlayerSfx(18, transform.position);
    }

    [PunRPC]
    public IEnumerator Protection()
    {
        SoundManager.Instance.PlayPlayerSfx(19, transform.position);
        earthShield = playerMaxHp * 0.1f;
        earthShieldEF.SetActive(true);
        yield return new WaitForSeconds(5f);
        earthShield = 0;
        earthShieldEF.SetActive(false);
    }

    [PunRPC]
    public void ProtectionBroken()
    {
        earthShieldEF.SetActive(false);
    }

    GameObject golem = null;
    [PunRPC]
    public void Guardian(Vector3 _sumonPos)
    {
        if (pv.IsMine)
        {
            golem = PhotonNetwork.Instantiate("Golem", _sumonPos, transform.rotation);
            golem.GetComponent<Golem>().maxHp = playerMaxHp * 1.5f;
        }
    }

    Vector3 fusionSkillPos;
    [PunRPC]
    public void EarthAndFire(Vector3 _pos)
    {
        fusionSkillPos = _pos;
        transform.rotation = Quaternion.LookRotation(_pos - transform.position);
        if (pv.IsMine)
        {
            pv.RPC("PlayTriggerAnimation", RpcTarget.All, "E&F");
        }
    }

    public void EarthAndFireFusionAni()
    {
        GameObject skill = Instantiate(crystal, fusionSkillPos, crystal.transform.rotation);
    }

    [PunRPC]
    public void EarthAndWater()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.zero - transform.position);
        if (pv.IsMine)
        {
            pv.RPC("PlayTriggerAnimation", RpcTarget.All, "E&W");
        }
    }

    public override float OtherShield(float _damage)
    {
        float damage = _damage;
        if (earthShield != 0)
        {
            if (earthShield < _damage)
            {
                damage = _damage - earthShield;
                earthShield = 0;
                pv.RPC("ProtectionBroken", RpcTarget.All, null);
            }
            else
            {
                earthShield -= _damage;
                damage = 0;
            }
        }
        return damage;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
            stream.SendNext(earthShield);
        }
        else
        {
            earthShield = (float)stream.ReceiveNext();
        }
    }

}
