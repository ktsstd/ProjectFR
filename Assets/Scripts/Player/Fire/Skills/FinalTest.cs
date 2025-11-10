using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTest : PlayerSkill
{
    public float damage;

    List<GameObject> monsterInRange = new List<GameObject>();
    public GameObject[] monsters;
    float damageDelay = 0f;

    public GameObject targetMonster;
    private void Start()
    {
        monsters = GameObject.FindGameObjectsWithTag("Enemy");

        Invoke("SelfDestroy", 8f);
    }

    void Update()
    {
        if (damageDelay >= 0)
            damageDelay -= Time.deltaTime;

        if (targetMonster != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMonster.transform.position, 5 * Time.deltaTime);
        }

        if (damageDelay <= 0)
        {
            if (targetMonster == null || targetMonster.activeSelf == false)
            {
                targetMonster = GetMinDistanceMonster();
            }

            foreach (GameObject monsters in monsterInRange)
            {
                if (monsters == null)
                    monsterInRange.Remove(monsters);

                HitOther(monsters, 100f + (damage * 0.2f));
            }

            damageDelay = 0.5f;
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

    GameObject GetMinDistanceMonster()
    {
        GameObject closeMonster = null;

        float min = 99;

        foreach (GameObject monster in monsters)
        {
            if (monster == null)
                continue;
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < min)
            {
                min = distance;
                closeMonster = monster;
            }
        }
        return closeMonster;
    }
}
