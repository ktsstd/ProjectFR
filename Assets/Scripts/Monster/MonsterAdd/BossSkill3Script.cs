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
    
    public void Starting(float Time)
    {
        thiscollider.enabled = false;
        if (Time > 0)
        {
            attackDuration = Time;
        }
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
    private void OnTriggerEnter(Collider other)
    {
        if (bossScript.swallowedTarget.Contains(other.gameObject)) return;

        if (other.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            bossScript.swallowedTarget.Add(other.gameObject);
            bossScript.photonView.RPC("Skill3Success", RpcTarget.All);
            thiscollider.enabled = false;
            gameObject.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
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
