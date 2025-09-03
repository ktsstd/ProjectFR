using System.Collections;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UIElements;

public class BossSkill1Script : MonoBehaviour
{
    [SerializeField] GameObject BossSkill1;
    //[SerializeField] GameObject BossHit;
    private float damage;
    bool isFadeIn = false;
    bool isdamaged = false;
    Drog bossScript;
    // Start is called before the first frame update
    public void Starting()
    {
        bossScript = GetComponentInParent<Drog>();
        damage += 50 + (bossScript.damage / 2);
        isdamaged = false;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; 
        float fadedTime = 0.93f; 

        while (elapsedTime <= fadedTime)
        {
            gameObject.GetComponent<MeshRenderer>().material.color
                = new Color(1, 0, 0, Mathf.Lerp(0, 0.8f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFadeIn = true;
        SoundManager.Instance.PlayMonsterSfx(3, transform.position); // Edit
        Invoke("DestroyBoundary", 0.2f);
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
                playerS.pv.RPC("OnPlayerHit", RpcTarget.All, damage);
            }
        }
    }
    void DestroyBoundary()
    {
        gameObject.SetActive(false);
        Vector3 currentEulerAngles = transform.eulerAngles;
        GameObject Skill1Obj = Instantiate(BossSkill1, transform.position,
            Quaternion.Euler(-90, currentEulerAngles.y, currentEulerAngles.z));
        bossScript.skillTimer[0] = bossScript.skillCooldown[0];
        bossScript.attackTimer = bossScript.attackCooldown;
        bossScript.currentState = Drog.States.Idle;
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        isFadeIn = false;
    }
}
