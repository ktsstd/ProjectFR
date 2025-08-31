using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossSkill4Script : MonoBehaviour
{
    [SerializeField] GameObject BossSkill4;
    [SerializeField] Drog bossScript;
    //bool isFadeIn = false;
    
    // Start is called before the first frame update
    public void Starting()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 1.2f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            gameObject.GetComponent<MeshRenderer>().material.color
                = new Color(1, 0, 0, Mathf.Lerp(0, 0.8f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //isFadeIn = true;
        Invoke("StartEffect", 0.2f);
        yield break;
    }

    void StartEffect()
    {
        gameObject.SetActive(false);
        Vector3 currentEulerAngles = transform.eulerAngles;
        GameObject Skill4Obj = Instantiate(BossSkill4, transform.position,
            Quaternion.Euler(-90, currentEulerAngles.y + 1f, currentEulerAngles.z));
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        //isFadeIn = false;
    }
}
