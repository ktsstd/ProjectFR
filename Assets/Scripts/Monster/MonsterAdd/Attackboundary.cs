using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attackboundary : MonoBehaviour
{
    GameObject attackboundaryObj;
    bool isFadeIn = false;
    MonsterAI monsterAIScript;
    float attackDuration = 4f;
    // Start is called before the first frame update
    void Start()
    {
        attackboundaryObj = gameObject;
        monsterAIScript = GetComponentInParent<MonsterAI>();

        // 부모의 Animator에서 "Attack" 애니메이션 클립의 길이 구하기
        Animator animator = monsterAIScript.animator;
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Attack")
                {
                    attackDuration = clip.length - 0.5f;
                    break;
                }
            }
        }
        else
        {
            Debug.Log("NO Animator");
        }

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = attackDuration; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            attackboundaryObj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFadeIn = true;
        Invoke("DestroyBoundary", 0.2f);
        yield break;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFadeIn && other.CompareTag("Player"))
        {
            PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerHit", RpcTarget.All, monsterAIScript.monsterInfo.damage);
            monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
            monsterAIScript.canMove = true;
            Destroy(this.gameObject);
        }
        else if (isFadeIn && other.CompareTag("Object"))
        {
            Object objectS = other.gameObject.GetComponent<Object>();
            objectS.photonView.RPC("Damaged", RpcTarget.All, monsterAIScript.monsterInfo.damage);
            monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
            monsterAIScript.canMove = true;
            Destroy(this.gameObject);
        }
    }

    void DestroyBoundary()
    {
        monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
        monsterAIScript.canMove = true;
        Destroy(this.gameObject);
    }
}
