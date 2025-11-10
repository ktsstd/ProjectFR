using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellingWave : PlayerSkill
{
    public Vector3 targetPos;
    public float damage;

    public GameObject waterEF;


    void Start()
    {
        Invoke("WaterEF", 0.8f);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 13f * Time.deltaTime);

        if (transform.position == targetPos)
        {
            Destroy(gameObject);
        }
    }

    void WaterEF()
    {
        Quaternion fireRot = transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 180));
        Instantiate(waterEF, transform.position + Vector3.up * 0.05f, fireRot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            MonsterAI monster = other.GetComponent<MonsterAI>();
            HitOther(other.gameObject, 120f + (damage * 0.25f));

            monster.OnMonsterSpeedDown(2f, 3f);
        }
    }
}
