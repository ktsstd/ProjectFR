using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShockSpin : MonoBehaviour
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
            other.GetComponent<MonsterAI>().MonsterDmged(100f + (damage * 0.2f));

            GameObject damageText = PoolManager.Instance.text_Pools.Get();
            damageText.transform.position = other.transform.position;
            damageText.GetComponent<DamageText>().damage = 100f + (damage * 0.2f);

            Instantiate(lightningHitEF, other.transform);
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
