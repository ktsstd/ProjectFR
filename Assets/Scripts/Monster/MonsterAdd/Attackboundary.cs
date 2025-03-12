using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class Attackboundary : MonoBehaviour
{
    GameObject attackboundaryObj;
    public int damage;
    bool isFadeIn = false;
    MonsterAI monsterAIScript;
    // Start is called before the first frame update
    void Start()
    {
        attackboundaryObj = gameObject;
        monsterAIScript = GetComponentInParent<MonsterAI>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 3f; // 총 소요 시간

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
            MonsterAI monsterAIScript = GetComponentInParent<MonsterAI>();
            monsterAIScript.AttackSuccess(other.gameObject, damage);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    void DestroyBoundary()
    {
        monsterAIScript.monsterInfo.attackTimer = monsterAIScript.monsterInfo.attackCooldown;
        monsterAIScript.canMove = true;
        Debug.Log("attack failed");
        PhotonNetwork.Destroy(this.gameObject);
    }
}
