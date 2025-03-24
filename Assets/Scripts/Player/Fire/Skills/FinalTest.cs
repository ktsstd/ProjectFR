using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTest : MonoBehaviour
{
    public float damage;

    List<GameObject> monsterInRange = new List<GameObject>();
    MonsterAI[] monsters;
    float damageDelay = 0f;

    MonsterAI targetMonster;

    private void Start()
    {
        monsters = FindObjectsOfType<MonsterAI>();

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
                monsters.GetComponent<MonsterAI>().MonsterDmged(100f + (damage * 0.2f));
            }

            if (targetMonster == null)
            {
                targetMonster = GetMinDistanceMonster();
            }

            damageDelay = 0.5f;
        }

        if (targetMonster != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMonster.transform.position, 5 * Time.deltaTime);
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

    MonsterAI GetMinDistanceMonster()
    {
        MonsterAI closeMonster = null;

        float min = 99;

        foreach (MonsterAI monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < min)
            {
                min = distance;
                closeMonster = monster;
            }
        }
        return closeMonster;
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
