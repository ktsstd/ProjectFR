using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BossSkill2Script : MonoBehaviour
{
    [SerializeField] GameObject BossSkill2;
    [SerializeField] GameObject BossHit;
    private float damage;
    bool isFadeIn = false;
    bool isdamaged = false;
    Drog bossScript;

    void Start()
    {
        bossScript = GetComponentInParent<Drog>();
        damage += 250 + (bossScript.monsterInfo.damage / 2);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 1f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            gameObject.GetComponent<MeshRenderer>().material.color 
                = new Color(1, 0, 0, Mathf.Lerp(0, 0.8f, elapsedTime / fadedTime));

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
            if (!isdamaged)
            {
                isdamaged = true;
                GameObject HitEffect
                = Instantiate(BossHit, PlayerObj,
                Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
                HitEffect.transform.SetParent(other.transform);
                playerS.pv.RPC("OnPlayerHit", RpcTarget.All, damage);
                PlayerObj = new Vector3(PlayerObj.x + 0.00001f, PlayerObj.y - 1f, PlayerObj.z);
                other.transform.position = PlayerObj;
            }
        }
    }

    void StartEffect()
    {
        Vector3 currentEulerAngles = transform.eulerAngles;
        GameObject Skill1Obj = Instantiate(BossSkill2, transform.position, 
            Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
        bossScript.BossMonsterSkillTimers[1] = bossScript.BossMonsterSkillCooldowns[1];
        bossScript.monsterInfo.attackTimer = bossScript.monsterInfo.attackCooldown;
        Collider collider = bossScript.GetComponent<Drog>().GetComponent<Collider>();
        collider.enabled = true;
        bossScript.canMove = true;
        DestroyImmediate(gameObject);
    }
}
