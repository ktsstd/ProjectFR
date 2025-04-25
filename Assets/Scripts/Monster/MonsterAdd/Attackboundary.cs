using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attackboundary : MonoBehaviour
{
    bool isFadeIn = false;
    [SerializeField] MonsterAI monsterAIScript;
    float attackDuration = 2f;
    // Start is called before the first frame update
    public void Starting()
    {
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
                else if (clip.name == "SolbornAttack")
                {
                    attackDuration = 1.5f;
                    break;
                }
                else if (clip.name == "Sleebam_Attack")
                {
                    attackDuration = 1.5f;
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
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFadeIn = true;
        Invoke("DestroyBoundary", 0.12f);
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
            gameObject.SetActive(false);
        }
        else if (isFadeIn && other.CompareTag("Object"))
        {
            Object objectS = other.gameObject.GetComponent<Object>();
            objectS.photonView.RPC("Damaged", RpcTarget.All, monsterAIScript.monsterInfo.damage);
            monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
            monsterAIScript.canMove = true;
            gameObject.SetActive(false);
        }
    }

    void DestroyBoundary()
    {
        monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
        monsterAIScript.canMove = true;
        isFadeIn = false;
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        gameObject.SetActive(false);
    }
}
