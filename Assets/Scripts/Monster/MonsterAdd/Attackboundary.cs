using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attackboundary : MonoBehaviour
{
    GameObject attackboundaryObj;
    bool isFadeIn = false;
    // Start is called before the first frame update
    void Start()
    {
        attackboundaryObj = gameObject;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 5f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            attackboundaryObj.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFadeIn && other.CompareTag("Player"))
        {
            MonsterAI monsterAIScript = GetComponentInParent<MonsterAI>();
            monsterAIScript.AttackSuccess();
            PhotonNetwork.Destroy(gameObject);
        }
        else if (isFadeIn)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
