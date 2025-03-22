using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSprayTest : MonoBehaviour
{
    public float damage;

    float damageDelay = 0f;
    void Update()
    {
        if (damageDelay >= 0)
            damageDelay -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.gameObject.TryGetComponent(out MonsterAI monster))
            {
                if (damageDelay <= 0)
                {
                    Debug.Log(80 + (damage * 0.1) + " 피해 입힘");
                    // 화상 추가
                    damageDelay = 0.5f;
                }
            }
        }
    }
}
