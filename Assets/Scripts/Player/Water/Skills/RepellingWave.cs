using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellingWave : MonoBehaviour
{
    public Vector3 targetPos;
    public float damage;

    public GameObject waterHitEF;


    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 13f * Time.deltaTime);

        if (transform.position == targetPos)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            MonsterAI monster = other.GetComponent<MonsterAI>();
            monster.MonsterDmged(120f + (damage * 0.25f));
            Instantiate(waterHitEF, other.transform);
            monster.OnMonsterSpeedDown(2f, 3f);
        }
    }
}
