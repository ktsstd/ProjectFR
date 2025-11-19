using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderTempo : PlayerSkill
{
    public float damage;

    float damageDelay = 0f;
    List<GameObject> monsterInRange = new List<GameObject>();

    int count = 0;

    public override void Start()
    {
        base.Start();
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

                    HitOther(monsters, 180f + damage * 0.1f);
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
}
