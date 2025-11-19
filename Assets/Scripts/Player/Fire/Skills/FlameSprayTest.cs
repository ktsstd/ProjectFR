using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSprayTest : PlayerSkill
{
    public float damage;

    List<GameObject> monsterInRange = new List<GameObject>();

    float damageDelay = 0f;

    public override void Start()
    {
        base.Start();
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

                HitOther(monsters, 80f + (damage * 0.1f));
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
