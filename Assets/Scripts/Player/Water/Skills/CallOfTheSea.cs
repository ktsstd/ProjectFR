using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallOfTheSea : MonoBehaviour
{
    public GameObject flameGrenadeTest;

    public float damage;

    float damageDelay = 0f;
    List<GameObject> monsterInRange = new List<GameObject>();

    void Start()
    {
        Invoke("SelfDestroy", 10f);
    }

    void Update()
    {
        if (damageDelay >= 0)
            damageDelay -= Time.deltaTime;

        if (damageDelay <= 0)
        {
            foreach (GameObject monsters in monsterInRange)
            {
                monsters.GetComponent<MonsterAI>().MonsterDmged(80f + damage * 0.1f);
            }
            damageDelay = 0.5f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.gameObject.TryGetComponent(out MonsterAI monster))
            {
                if (!monster.monsterInfo.isBoss)
                    monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, Time.deltaTime * 5f);
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
