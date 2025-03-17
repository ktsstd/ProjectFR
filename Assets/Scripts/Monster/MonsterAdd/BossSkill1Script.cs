using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossSkill1Script : MonoBehaviour
{
    GameObject attackboundaryObj;
    public int damage;
    bool isFadeIn = false;
    Boss bossScript;
    // Start is called before the first frame update
    void Start()
    {
        attackboundaryObj = gameObject;
        bossScript = GetComponentInParent<Boss>();
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
        Invoke("StartEffect", 0.2f);
        yield break;
    }
    private void OnTriggerStay(Collider other)
    {
        if (isFadeIn && other.CompareTag("Player"))
        {
            bossScript.Skill1Success(other.gameObject, damage);
        }
    }

    void StartEffect()
    {
        Vector3 currentEulerAngles = transform.eulerAngles;
        GameObject Skill1Obj = PhotonNetwork.Instantiate("Boss_Skill_01_Prefab", transform.position, Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
        // Skill1Obj.transform.SetParent(this.transform);
        bossScript.BossMonsterSkillTimers[0] = bossScript.BossMonsterSkillCooldowns[0];
        bossScript.monsterInfo.attackTimer = bossScript.monsterInfo.attackCooldown;
        bossScript.canMove = true;
        Destroy(gameObject);
    }
}
