using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderTempo : MonoBehaviour
{
    public float damage;

    float damageDelay = 0f;
    List<GameObject> monsterInRange = new List<GameObject>();

    public GameObject lightningHitEF;

    int count = 0;

    void Start()
    {
        Invoke("SelfDestroy", 2f);
    }

    void Update()
    {
        if (count <= 8)
        {
            if (damageDelay >= 0)
                damageDelay -= Time.deltaTime;

            if (damageDelay <= 0)
            {
                foreach (GameObject monsters in monsterInRange)
                {
                    if (monsters == null)
                        monsterInRange.Remove(monsters);

                    monsters.GetComponent<MonsterAI>().MonsterDmged(180f + damage * 0.1f);

                    GameObject damageText = PoolManager.Instance.text_Pools.Get();
                    damageText.transform.position = monsters.transform.position;
                    damageText.GetComponent<DamageText>().damage = 180f + damage * 0.1f;

                    Instantiate(lightningHitEF, monsters.transform);
                }
                damageDelay = 0.15f;
                count++;
            }
        }
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

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
