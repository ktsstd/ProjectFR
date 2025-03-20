using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBomb : MonoBehaviour
{
    GameObject attackboundaryObj;
    public float damage;
    bool isFadeIn = false;
    MonsterAI monsterAIScript;
    GameObject MonsterObj;
    float attackDuration = 4f;
    // Start is called before the first frame update
    void Start()
    {
        attackboundaryObj = gameObject;
        monsterAIScript = GetComponentInParent<MonsterAI>();
        MonsterObj = monsterAIScript.gameObject;

        // �θ��� Animator���� "Attack" �ִϸ��̼� Ŭ���� ���� ���ϱ�
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
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = attackDuration; // �� �ҿ� �ð�

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
        if (isFadeIn && other.CompareTag("Object"))
        {
            //PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            //playerS.photonView.RPC("OnPlayerHit", RpcTarget.All, damage);
            Debug.Log("suc");
        }
    }

    void DestroyBoundary()
    {
        monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
        monsterAIScript.canMove = true;
        Destroy(MonsterObj.gameObject);
    }
}
