using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumonDamage : MonoBehaviour
{
    public float damage;
    float attackTime = 1;

    public void Update()
    {
        if (attackTime >= 0)
            attackTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attackTime > 0)
        {
            if (other.tag == "Enemy")
            {
                MonsterAI monster = other.gameObject.GetComponent<MonsterAI>();
                monster.MonsterDmged(150f + damage * 0.2f);
                monster.OnMonsterStun(1f);
            }
        }
    }
}
