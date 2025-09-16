using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Lightning : PlayerController
{
    public GameObject[] playerRenderer;

    public GameObject speedUpEF;

    int skilltype;
    public GameObject shockSpin;
    public GameObject thunderRush;
    public GameObject thunderTempo;

    public GameObject thunderRushEF;

    public GameObject dagger;

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

    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (currentStates == States.Dash)
        {
            if (collision.gameObject.tag == "Enemy")
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
                        Vector3 direction = GetMousePosition() - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;

                        skillRanges[1].transform.position = transform.position + direction * 5;
                        skillRanges[1].transform.rotation = Quaternion.LookRotation(GetSkillRange(5) - transform.position) * Quaternion.Euler(new Vector3(0, 90, 0));
                    }
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        Vector3 direction = GetMousePosition() - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;
                        skillsPos[1] = transform.position + direction * 5;
                        movePos = transform.position + direction * 10;
                        transform.rotation = Quaternion.LookRotation(GetSkillRange(5) - transform.position);
                        currentSkillsCoolTime[1] = skillsCoolTime[1];
                        skillRanges[1].SetActive(false);
                        currentSkillsCoolTime[1] = skillsCoolTime[1];
                        currentStates = States.Attack;
                        skilltype = 0;
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
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        skillRanges[2].SetActive(false);
                        currentSkillsCoolTime[2] = skillsCoolTime[2];
                        currentStates = States.Attack;
                        skilltype = 1;
                        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill2");
                    }
                }
            }
            ElementalSetting();
        }
    }

    public void StopAnimation()
    {
        currentStates = States.Idle;
    }

    public void AniReset()
    {
        animator.ResetTrigger("skill2_2");
        animator.ResetTrigger("skill3");
    }

    Vector3 movePos;
    public void StopAttackAnimation()
    {
        if (skilltype == 0)
        {
            if (pv.IsMine)
            {
                pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill2_2");
                pv.RPC("ThunderRush", RpcTarget.All, skillsPos[1]);
                transform.position = movePos;
                currentStates = States.Idle;
            }
        }
        else
        {
            if (pv.IsMine)
            {
                pv.RPC("ThunderTempo", RpcTarget.All, null);
            }
        }
    }

    public void UseShockSpin()
    {
        if (pv.IsMine)
            pv.RPC("ShockSpin", RpcTarget.All, null);
    }

    public void UseThunderRush()
    {
        if (pv.IsMine)
            pv.RPC("ThunderRush", RpcTarget.All, skillsPos[1]);
    }

    [PunRPC]
    public void ShockSpin()
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(90, 0, -20));
        GameObject skill = Instantiate(shockSpin, transform.position, fireRot);
        skill.GetComponent<ShockSpin>().damage = playerAtk;
        SoundManager.Instance.PlayPlayerSfx(4, transform.position);
    }

    [PunRPC]
    public void ThunderRush(Vector3 _targetPos)
    {
        Instantiate(thunderRushEF, transform);
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(90, 90, 0));
        GameObject skill = Instantiate(thunderRush, _targetPos, fireRot);
        skill.GetComponent<ThunderRush>().damage = playerAtk;
        SoundManager.Instance.PlayPlayerSfx(5, transform.position);
    }

    [PunRPC]
    public void ThunderTempo()
    {
        rigidbody.useGravity = false;
        collider.enabled = false;
        foreach (GameObject playerSkin in playerRenderer)
            playerSkin.SetActive(false);
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        GameObject skill = Instantiate(thunderTempo, transform.position + (Vector3.up * 0.5f), fireRot);
        skill.GetComponent<ThunderTempo>().damage = playerAtk;
        SoundManager.Instance.PlayPlayerSfx(6, transform.position);
        Invoke("StateReset", 1.5f);
    }

    Vector3 fusionSkillPos;
    [PunRPC]
    public void LightningAndFire(Vector3 _pos)
    {
        fusionSkillPos = _pos;
        transform.rotation = Quaternion.LookRotation(_pos - transform.position);
        if (pv.IsMine)
        {
            pv.RPC("PlayTriggerAnimation", RpcTarget.All, "L&F");
        }
    }

    public void LightningFusionAni()
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
        GameObject skill = Instantiate(dagger, transform.position, fireRot);
        skill.GetComponent<Dagger>().target = fusionSkillPos;
    }

    [PunRPC]
    public void LightningAndWater()
    {
        fusionSkillPos = transform.position + Vector3.up * 10;
        if (pv.IsMine)
        {
            pv.RPC("PlayTriggerAnimation", RpcTarget.All, "L&W");
        }
    }

    public void StateReset()
    {
        rigidbody.useGravity = true;
        collider.enabled = true;
        foreach (GameObject playerSkin in playerRenderer)
            playerSkin.SetActive(true);
        if (currentStates != States.Die)
            pv.RPC("PlayTriggerAnimation", RpcTarget.All, "skill3");
    }
}