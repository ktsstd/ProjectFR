using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellingWave : MonoBehaviour
{
    public Vector3 targetPos;
    public float damage;


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
            Debug.Log(120 + (damage * 0.25) + " 피해 입힘 + 슬로우 필요");
        }
        // 데미지 주기 + 슬로우
    }
}
