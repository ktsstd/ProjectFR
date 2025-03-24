using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class BossSkill1Script : MonoBehaviour
{
    GameObject attackboundaryObj;
    [SerializeField] GameObject BossSkill1;
    [SerializeField] GameObject BossHit;
    private float damage;
    bool isFadeIn = false;
    Boss bossScript;
    // Start is called before the first frame update
    void Start()
    {
        attackboundaryObj = gameObject;
        bossScript = GetComponentInParent<Boss>();
        damage += 50 + (bossScript.monsterInfo.damage / 2);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 0.93f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            attackboundaryObj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFadeIn = true;
        Invoke("StartEffect", 0.2f);
        yield break;
    }
    private void OnTriggerStay(Collider other)
    {
        if (isFadeIn && other.CompareTag("Player"))
        {
            PlayerController playerS = other.gameObject.GetComponent<PlayerController>();
            Vector3 PlayerObj = other.transform.position;
            PlayerObj.y += 1f;
            Vector3 currentEulerAngles = transform.eulerAngles;
            GameObject HitEffect = Instantiate(BossHit, PlayerObj, Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
            playerS.pv.RPC("OnPlayerHit", RpcTarget.All, damage);
        }
    }

    void StartEffect()
    {
        Vector3 currentEulerAngles = transform.eulerAngles;
        GameObject Skill1Obj = Instantiate(BossSkill1, transform.position, Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
        bossScript.BossMonsterSkillTimers[0] = bossScript.BossMonsterSkillCooldowns[0];
        bossScript.monsterInfo.attackTimer = bossScript.monsterInfo.attackCooldown;
        bossScript.canMove = true;
        DestroyImmediate(gameObject);
    }
}
