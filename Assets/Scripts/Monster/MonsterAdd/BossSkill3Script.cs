using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MeshFilter))]
public class BossSkill3Script : MonoBehaviour {
    private float radius = 7f;
    private float angle = 165f;
    private int segments = 50;
    bool isFadeIn = false;
    [SerializeField] Drog bossScript;
    private GameObject[] SuccessPlayer;
    public void Starting()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float fadedTime = 0.9f; 

        while (elapsedTime <= fadedTime)
        {
            gameObject.GetComponent<MeshRenderer>().material.color 
                = new Color(1, 0, 0, Mathf.Lerp(0, 0.8f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFadeIn = true;
        Invoke("DestroyBoundary", 0.2f);
        yield break;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFadeIn && other.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            GameObject rootPlayerObj = other.transform.root.gameObject;
            if (!bossScript.FSkill3Obj.Contains(rootPlayerObj))
            {
                bossScript.FSkill3Obj.Add(rootPlayerObj);
                bossScript.F3Skill3Script.Add(rootPlayerObj.GetComponent<PlayerController>());
                bossScript.photonView.RPC("Skill3Success", RpcTarget.All);
            }
            isFadeIn = false;
            gameObject.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        }
    }

    void DestroyBoundary()
    {
        if (isFadeIn)
        {
            isFadeIn = false;
            gameObject.SetActive(false);
            bossScript.animator.SetTrigger("Skill3Over");
            bossScript.monsterInfo.attackTimer = bossScript.monsterInfo.attackCooldown;
            bossScript.BossMonsterSkillTimers[2] = bossScript.BossMonsterSkillCooldowns[2];
            bossScript.canMove = true;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        }
        
    }
}
