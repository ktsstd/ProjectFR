using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthFire : MonoBehaviour
{
    float damageDelay = 0f;
    List<GameObject> monsterInRange = new List<GameObject>();

    void Start()
    {
        Invoke("SelfDestroy", 8f);
    }

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

                monsters.GetComponent<MonsterAI>().MonsterDmged(125f);
            }
            damageDelay = 1f;
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
