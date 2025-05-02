using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MeshFilter))]
public class BossSkill3Script : MonoBehaviour
{
    bool isFadeIn = false;
    [SerializeField] Drog bossScript;
    [SerializeField] Collider thiscollider;
    float attackDuration = 0.9f;
    //GameObject rootPlayerObj;
    public void Starting(float Time)
    {
        if (Time > 0)
        {
            attackDuration = Time;
        }
        //rootPlayerObj = null;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float fadedTime = attackDuration;

        while (elapsedTime <= fadedTime)
        {
            gameObject.GetComponent<MeshRenderer>().material.color
                = new Color(1, 0, 0, Mathf.Lerp(0, 0.8f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //isFadeIn = true;
        thiscollider.enabled = true;
        Invoke("DestroyBoundary", 0.12f);
        yield break;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (isFadeIn && other.CompareTag("Player"))
    //    {
    //        rootPlayerObj = other.transform.root.gameObject;
    //        if (!bossScript.FSkill3Obj.Contains(rootPlayerObj))
    //        {
    //            bossScript.FSkill3Obj.Add(rootPlayerObj);
    //            bossScript.F3Skill3Script.Add(rootPlayerObj.GetComponent<PlayerController>());
    //            bossScript.photonView.RPC("Skill3Success", RpcTarget.All);
    //        }
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            //bossScript.Skill3Obj.Add(other.gameObject);
            bossScript.photonView.RPC("Skill3Success", RpcTarget.All);
            thiscollider.enabled = false;
        }
    }

    void DestroyBoundary()
    {
        if (thiscollider.enabled)
        {
            thiscollider.enabled = false;
            gameObject.SetActive(false);
            bossScript.animator.SetTrigger("Skill3Over");
            bossScript.attackTimer = bossScript.attackCooldown;
            bossScript.BossMonsterSkillTimers[2] = bossScript.BossMonsterSkillCooldowns[2];
            bossScript.canMove = true;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        }
        //if (isFadeIn)
        //{
        //    isFadeIn = false;
        //    gameObject.SetActive(false);
        //    if (!bossScript.is3Patterning)
        //    {
        //        bossScript.animator.SetTrigger("Skill3Over");
        //        bossScript.attackTimer = bossScript.attackCooldown;
        //        bossScript.BossMonsterSkillTimers[2] = bossScript.BossMonsterSkillCooldowns[2];
        //        bossScript.canMove = true;
        //    }
        //    gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        //} 
    }
}
