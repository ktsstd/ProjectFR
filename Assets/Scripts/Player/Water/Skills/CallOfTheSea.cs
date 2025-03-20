using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallOfTheSea : MonoBehaviour
{
    public float damage;

    float damageDelay = 0f;
    int count = 0;

    void Start()
    {
        Invoke("SelfDestroy", 10f);
    }

    void Update()
    {
        if (damageDelay >= 0)
            damageDelay -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.gameObject.TryGetComponent(out MonsterAI monster))
            {
                if (damageDelay <= 0)
                {
                    Debug.Log(80 + (damage * 0.1) + " ÇÇÇØ ÀÔÈû");
                    damageDelay = 0.5f;
                }
                if (!monster.monsterInfo.isBoss)
                    monster.transform.position = Vector3.MoveTowards(monster.transform.position, transform.position, Time.deltaTime * 5f);
            }
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
