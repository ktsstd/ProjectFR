using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSprayTest : MonoBehaviour
{
    public float damage;

    List<GameObject> monsterInRange = new List<GameObject>();

    public GameObject fireHitEF;

    float damageDelay = 0f;
    void Update()
    {
        if (damageDelay >= 0)
            damageDelay -= Time.deltaTime;

        if (damageDelay <= 0)
        {
            foreach (GameObject monsters in monsterInRange)
            {
                if (monsters == null)
                    monsterInRange.Remove(monsters);

                monsters.GetComponent<MonsterAI>().MonsterDmged(80f + (damage * 0.1f));

                GameObject damageText = PoolManager.Instance.text_Pools.Get();
                damageText.transform.position = monsters.transform.position;
                damageText.GetComponent<DamageText>().damage = 80f + (damage * 0.1f);

                Instantiate(fireHitEF, monsters.transform);
            }
            damageDelay = 0.5f;
        }
    }

    private void OnDisable()
    {
        monsterInRange.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            monsterInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            monsterInRange.Remove(other.gameObject);
        }
    }
}
