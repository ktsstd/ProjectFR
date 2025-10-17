using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRush : MonoBehaviour
{
    public float damage;
    public GameObject lightningHitEF;

    void Start()
    {
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<MonsterAI>().MonsterDmged(60f + (damage * 0.5f));

            GameObject damageText = PoolManager.Instance.text_Pools.Get();
            damageText.transform.position = other.transform.position;
            damageText.GetComponent<DamageText>().damage = 60f + (damage * 0.5f);

            Instantiate(lightningHitEF, other.transform);
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
