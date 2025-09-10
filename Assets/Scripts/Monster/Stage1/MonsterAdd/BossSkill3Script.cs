using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MeshFilter))]
public class BossSkill3Script : MonoBehaviour
{
    [SerializeField] Drog bossScript;
    [SerializeField] Collider thiscollider;
    float attackDuration = 0.9f;
    
    public void Starting(float Time)
    {
        thiscollider.enabled = false;
        bossScript.swallowedTarget.Clear();
        foreach (GameObject playerObj in bossScript.swallowedTarget)
        {
            Debug.Log(playerObj);
        }
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
        if (bossScript.swallowedTarget.Contains(other.gameObject) && !PhotonNetwork.IsMasterClient) return;

        if (other.CompareTag("Player"))
        {
            bossScript.photonView.RPC("AddSwallowedTarget", RpcTarget.All, other.gameObject.name);
            bossScript.photonView.RPC("Skill3Success", RpcTarget.All);
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        }
    }

    void DestroyBoundary()
    {
        thiscollider.enabled = false;
        gameObject.SetActive(false);
        if (bossScript.swallowedTarget.Count == 0)
        {            
            bossScript.animator.SetTrigger("Skill3Over");
            bossScript.thinkTimer = bossScript.thinkTime;
            bossScript.skillTimer[2] = bossScript.skillCooldown[2];
            bossScript.currentState = Drog.States.Idle;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        }
    }
}
